using BarberSalon.Data;
using BarberSalon.DTO;
using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Models.Enum;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services.Implements
{
    public class AdminService : IAdminService
    {
        public AppDbContext _db;
        public UserManager<ApplicationUser> _userManager;
        public AdminService(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<WorkingHour>> GetEmployeeSchedule(string employeeId)
        {
            Console.WriteLine("Searching employee: " + employeeId);

            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.UserId == employeeId);

            if (employee == null)
            {
                Console.WriteLine("NO EMPLOYEE FOUND");
                return new List<WorkingHour>();
            }

            Console.WriteLine("Employee Id: " + employee.Id);


            var allHours = await _db.WorkingHours.ToListAsync();

            foreach (var h in allHours)
            {
                Console.WriteLine(
                    $"WH Id:{h.Id} EmployeeId:{h.EmployeeId} Day:{h.Day}"
                );
            }


            var data = await _db.WorkingHours
                .Where(w => w.EmployeeId == employee.Id)
                .OrderBy(w => w.Day)
                .ToListAsync();


            Console.WriteLine("Hours found: " + data.Count);

            return data;
        }
        public async Task<string> GetEmployeeName(string employeeId)
        {
            return await _db.Employees.Select(e => e.UserId).FirstAsync();
        }
        public async Task<List<BarberSalon.Models.Category>> GetallCategory()
        {
            return await _db.Categories.Include(r=>r.Products).ToListAsync();
        }
        public async Task ToogleStatus(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
            {
                return;
            }

            category.IsActive = !category.IsActive;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> existCategory(string name)
        {
            var exist = await _db.Categories.AnyAsync(x => x.Name == name);
            return exist;

        }
        public async Task AddCategory(CreateCategoryBindingModel c)
        {
            var category = new BarberSalon.Models.Category()
            {
                Description = c.Description,
                IsActive = c.IsActive,
                Name = c.Name,
            };
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }
        public async Task<BarberSalon.Models.Category> LoadCategoryById(int id)
        {
            return await _db.Categories.FindAsync(id);
        }
        public async Task EditCategory(int id,CreateCategoryBindingModel s)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category is null)
            {
                return;
            }
            category.Description = s.Description;
            category.IsActive=s.IsActive;
            category.Name=s.Name;
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Product>> GetAllProducts()
        {
            return await _db.Products.Include(r=>r.Category).ToListAsync();
        }
        public async Task ToogleProductStatus(int id)
        {
            var pr = await _db.Products.FindAsync(id);
            if (pr is null)
            {
                return;
            }
            pr.IsActive = !pr.IsActive;
            // we dont add AddASync because EF do the tracking this one of most EF Characteristic Important :)
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Category>> GetallCategoryActive()
        {
            return await _db.Categories.Include(r=>r.Products).Where(x=>x.IsActive==true).ToListAsync();
        }
        public async Task AddProduct(BarberSalon.Models.Product s)
        {
            await _db.Products.AddAsync(s);
            await _db.SaveChangesAsync();
        }
        public async Task<BarberSalon.Models.Product> GetProductById(int id )
        {
            return await _db.Products.FindAsync(id);
        }
        public async Task<List<Employee>> GetEmployeesInDB()
        {
            return await _db.Employees.ToListAsync();
        }
        public async Task<List<WorkingHour>> LoadSchedule(int eid)
        {
            return await _db.WorkingHours.Where(x => x.EmployeeId == eid).AsNoTracking().OrderBy(s => s.Day).ToListAsync();
        }
        public async Task<WorkingHour> checkifExist(int EmployeeId, DayOfWeek day)
        {
          return  await _db.WorkingHours.FirstOrDefaultAsync(w =>w.EmployeeId == EmployeeId &&w.Day == day);
        }
        public async Task AddWorkingHours(WorkingHour d)
        {
            await _db.WorkingHours.AddAsync(d);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateWorkingHours(int id, WorkingHour s1)
        {
            var existing = await _db.WorkingHours.FindAsync(id);

            if (existing == null)
                return;
            Console.WriteLine($"Before: {existing.StartTime} - {existing.EndTime}");

            existing.StartTime = s1.StartTime;
            existing.EndTime = s1.EndTime;
            existing.IsOffDay = s1.IsOffDay;

            Console.WriteLine($"After: {existing.StartTime} - {existing.EndTime}");

            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Appointment>> LoadAppointments()
        {
            return await _db.Appointments
                         .Include(a => a.User)
                         .Include(a => a.Employee)
                         .Include(a => a.Service)
                         .OrderBy(a => a.AppointmentDate)
                         .ThenBy(a => a.StartTime)
                         .ToListAsync();
        }
        public async Task<BarberSalon.Models.Appointment> LoadAppointmentById(int id )
        {
            return await _db.Appointments.FindAsync(id);
        }
        public async Task UpdateAppointment(int id , AppointmentStatus status)
        {
            var app = await _db.Appointments.FindAsync(id);
            if (app is null)
            {
                return;
            }
            app.Status = status;
            await _db.SaveChangesAsync();
        }
        public async Task AddEmployee(BarberSalon.Models.Employee e)
        {
            await _db.AddAsync(e);
            await _db.SaveChangesAsync();
        }
        public async Task<BarberSalon.Models.Employee> LoadEmployeeById(int id)
        {
            return await _db.Employees.FindAsync(id);
        }
        public async Task Diactive(int id)
        {
            
            var x = await _db.Employees.FindAsync(id);
            if(x is null)
            {
                return;
            }
            x.IsActive = false;
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Employee>> LoadActiveEmployee()
        {
            return await _db.Employees.Where(x => x.IsActive).ToListAsync();
        }
        //services
        public async Task<List<BarberSalon.Models.Service>> GetAllService()
        {
            return await  _db.Services.OrderBy(s => s.Name).ToListAsync();
        }
        public async Task<BarberSalon.Models.Service> GetServiceById(int id )
        {
           return await _db.Services.FindAsync(id);
        }
        public async Task DeleteService(int id)
        {
            Console.WriteLine(id);

            var service = await _db.Services.FindAsync(id);

            if (service is null)
            {
                return;
            }

            service.IsActive = false;

            await _db.SaveChangesAsync();
        }
        public async Task AddService(BarberSalon.Models.Service s)
        {
            await _db.Services.AddAsync(s);
            await _db.SaveChangesAsync();
        }
        public async Task updateService(BarberSalon.Models.Service s)
        {
            var u = await _db.Services.FindAsync(s.Id);
            if(u is null)
            {
                return;
            }
            u.Name = s.Name;
            u.Description = s.Description;
            u.Price = s.Price;
            u.Duration = s.Duration;
            u.IsActive = s.IsActive;
            u.ImageUrl = s.ImageUrl;
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Order>> GetOrders()
        {
            return await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
        public async Task updateOrder(int id , OrderStatus status )
        {
            var x = await _db.Orders.FindAsync(id);
            if( x is null )
            {
                return;
            }
            x.Status = status;
            await _db.SaveChangesAsync();
        }
        /// Dashboard /// 
        public async Task<List<MonthlyRevenueDto>> GetMothlyRevenue()
        {
            return await _db.Payments
            .Where(p => p.PaymentStatus == StatusOfPayment.Paid)
            .GroupBy(p => p.PaymentDate.Month)
            .Select(g => new MonthlyRevenueDto
            {
                Month = g.Key,
                Revenue = g.Sum(x => x.Amount)
            })
            .OrderBy(x => x.Month)
            .ToListAsync();
        }
        public async Task<(int customerCount, int employeeCount, int orderCount, decimal revenue)> GetAllData()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");

            var customerCount = customers.Count;
            var employeeCount = await _db.Employees.CountAsync(e => e.IsActive);
            var orderCount = await _db.Orders.CountAsync(o => o.Status != OrderStatus.Cancelled);
            var revenue = await _db.Payments.Where(p => p.PaymentStatus == StatusOfPayment.Paid).SumAsync(p => p.Amount);
          
            return (customerCount, employeeCount, orderCount, revenue);
        }
       public async  Task<List<AppointmentStatusVM>> GetAppointmentStatusCount()
        {
            var result = await _db.Appointments
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentStatusVM
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
            return result;
        }

        public async Task<List<ServiceBookingVM>> GetMostBookedServices()
        {
            var result = await _db.Appointments
                .Include(a => a.Service)
                .GroupBy(a => a.Service.Name)
                .Select(g => new ServiceBookingVM
                {
                    ServiceName = g.Key,
                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToListAsync();
            return result;
        }
        public async Task<List<EmployeeBookingVM>> GetTopEmployees()
        {
            var result = await _db.Appointments

                .Include(a => a.Employee)
                .GroupBy(a => a.Employee.Name)
                .Select(g => new EmployeeBookingVM
                {
                    EmployeeName = g.Key,

                    BookingCount = g.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(5)
                .ToListAsync();


            return result;
        }
    }
}
