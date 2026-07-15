using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Models.Enum;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BarberSalon.Services.Implements
{
    public class CustomerService : ICustomerService
    {
        public AppDbContext _db;
        public IHttpContextAccessor _httpContextAccessor;
        public CustomerService(AppDbContext db, IHttpContextAccessor httpContext)
        {
            _db = db;
            _httpContextAccessor = httpContext;
        }

        public async Task<List<Service>> GetAllServices()
        {
            return await _db.Services.Where(x=>x.IsActive).ToListAsync();
        }
        public async Task<BarberSalon.Models.Service> LoadServiceById(int id)
        {
            return await _db.Services.FindAsync(id);
        }
        public async Task<List<string>> GetAvailableTimes(int employeeId, int serviceId, DateTime date)
        {
            date = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
            var workingHour = await _db.WorkingHours.FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.Day == date.DayOfWeek);
            // this mean we are off today 
            if (workingHour == null || workingHour.IsOffDay)
            {
                return new List<string>();
            }
            var service = await _db.Services.FirstAsync(x => x.Id == serviceId);
            // load the appointment 

            var appointments = await _db.Appointments.Where(x => x.EmployeeId == employeeId && x.AppointmentDate.Date == date.Date).ToListAsync();
            // now lets create the list of available time 
            List<string> available = new();
            TimeOnly current = workingHour.StartTime;
            // last client can accept since for example 8-4 the last client if need 30 min 
            // for example can attend 3:30 so the last if before Duration t from the end of day work
            TimeOnly last = workingHour.EndTime.AddMinutes(-service.Duration);
            while (current <= last)
            {
                // every time add duration time (we generate all the list then filter it )
                // 9-9:30 -- 9:30 - 10 ...each iteration represent a time 
                // if attend 3:30 the end on 4 ( 10:30 - 11 ) ..

                //current = current.AddMinutes(service.Duration);
                var end = current.AddMinutes(service.Duration);

                bool occupied = appointments.Any(a =>
                 current < a.EndTime &&
                 end > a.StartTime);
                if (!occupied)
                {
                    available.Add(current.ToString("HH:mm"));
                }
                current = current.AddMinutes(service.Duration);

            }
            return available;
        }
        public async Task BookAppointment(BarberSalon.Models.Appointment app)
        {

            app.AppointmentDate =
DateTime.SpecifyKind(app.AppointmentDate, DateTimeKind.Utc);
            app.Status = Models.Enum.AppointmentStatus.Pending;
            await _db.Appointments.AddAsync(app);

            await _db.SaveChangesAsync();

        }
        public async Task<List<Models.Product>> GetProductsByCategoryId(int id)
        {
            return await _db.Products.Where(x => x.CategoryId == id).ToListAsync();
        }
        public async Task<BarberSalon.Models.Category> GetCategoryById(int id)
        {
            return await _db.Categories.FindAsync(id);
        }
        public async Task<BarberSalon.Models.Product> getProductById(int id)
        {
            return await _db.Products.Include(r => r.Category).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddToCart(int id, int qty)
        {
            var Product = await _db.Products.FindAsync(id);

            if (Product == null)
            {
                return;
            }

            // Read json cart 
            var json = _httpContextAccessor.HttpContext!.Session.GetString("Cart");

            List<CartItem> cart;

            if (string.IsNullOrEmpty(json))
            {
                cart = new List<CartItem>();
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(json)!;
            }

            // Check if exist 
            var item = cart.FirstOrDefault(x => x.ProductId == Product.Id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = Product.Id,
                    Name = Product.Name,
                    Price = Product.Price,
                    ImageUrl = Product.ImageUrl,
                    Quantity = qty
                });
            }
            else
            {
                item.Quantity += qty;
            }
            _httpContextAccessor.HttpContext!.Session.SetString("Cart", JsonSerializer.Serialize(cart));

        }

        public async Task<bool> MakeTheOrder(ApplicationUser user, List<CartItem> cart, string paymentMethod)
        {
            try
            {
                // use try catch here for debuging 
                Order order = new Order
                {
                    UserId = user.Id,

                    OrderDate = DateTime.UtcNow,

                    TotalAmount = cart.Sum(x =>
                        x.Price * x.Quantity),

                    Status = OrderStatus.Pending
                };
                foreach (var item in cart)
                {

                    OrderItem orderItem = new OrderItem
                    {

                        ProductId = item.ProductId,

                        Quantity = item.Quantity,

                        Price = item.Price

                    };
                    var product = await _db.Products.FindAsync(item.ProductId);
                    if(product != null)
                    {
                        product.StockStatus -= item.Quantity;
                    }


                    order.OrderItems.Add(orderItem);

                }
                Payment payment = new Payment
                {

                    Amount = order.TotalAmount,

                    PaymentDate = DateTime.UtcNow,

                    PaymentMethod =
                    paymentMethod == "CreditCard" ? PaymentMethod.CreditCard : PaymentMethod.PayAtShop
                    ,
                    PaymentStatus = paymentMethod == "CreditCard" ? StatusOfPayment.Paid : StatusOfPayment.UnPaid

                };

                order.Payment = payment;

                _db.Orders.Add(order);

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

                return false;

            }

        }
        // Load Appointment  by id here 
        public async Task<List<BarberSalon.Models.Appointment>> GetAppointmentById(string id)
        {
            return await _db.Appointments
                   .Include(x => x.Service)
                   .Include(x => x.Employee)
                   .Where(x => x.UserId == id)
                   .OrderByDescending(x => x.AppointmentDate)
                   .ThenBy(x => x.StartTime)
                   .ToListAsync();
        }
        public async Task CancelAppointment(int id, string userId)
        {
            var app = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (app is null)
            {
                return; // sumilate to problem page 
            }
            app.Status = AppointmentStatus.Cancelled;
            await _db.SaveChangesAsync();
        }
        // my orders handlers
        public async Task<List<BarberSalon.Models.Order>> GetAllOrderByUser(string id)
        {
            return await _db.Orders.Include(x => x.Payment).Where(x => x.UserId == id).OrderByDescending(x => x.OrderDate).ToListAsync();
        }
        public async Task<BarberSalon.Models.Order> LoadOrderById(int id,string uid)
        {
           return await _db.Orders

             .Include(x => x.Payment)
             .Include(x => x.OrderItems)
             .ThenInclude(x => x.Product)
             .FirstOrDefaultAsync(x =>x.Id == id &&x.UserId == uid);
        }
    }
    }

