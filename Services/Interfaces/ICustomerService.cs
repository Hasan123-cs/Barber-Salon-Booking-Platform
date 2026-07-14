namespace BarberSalon.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<BarberSalon.Models.Service>> GetAllServices();
        public Task<BarberSalon.Models.Service> LoadServiceById(int id);
    }
}
