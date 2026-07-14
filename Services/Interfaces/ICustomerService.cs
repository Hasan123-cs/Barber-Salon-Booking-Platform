using BarberSalon.Models;

namespace BarberSalon.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<BarberSalon.Models.Service>> GetAllServices();
        public Task<BarberSalon.Models.Service> LoadServiceById(int id);
        public Task<List<string>> GetAvailableTimes(int employeeId,int serviceId,DateTime date);
        public Task BookAppointment(BarberSalon.Models.Appointment app);
        public  Task<List<Models.Product>> GetProductsByCategoryId(int id);
        public Task<BarberSalon.Models.Category> GetCategoryById(int id);
        public Task<BarberSalon.Models.Product> getProductById(int id);
        public Task AddToCart(int id, int qty);
        Task<bool> MakeTheOrder(ApplicationUser user,List<CartItem> cart,string paymentMethod);
        public  Task<List<BarberSalon.Models.Appointment>> GetAppointmentById(string id);
        public Task CancelAppointment(int id, string userId);
        public Task<List<BarberSalon.Models.Order>> GetAllOrderByUser(string id);
        public Task<BarberSalon.Models.Order> LoadOrderById(int id, string uid);








    }
}
