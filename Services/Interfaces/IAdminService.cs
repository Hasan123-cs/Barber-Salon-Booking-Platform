using BarberSalon.Models;
using BarberSalon.Models.BindingModel;

namespace BarberSalon.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<WorkingHour>> GetEmployeeSchedule(string employeeId);
        public Task<string> GetEmployeeName(string employeeId);
        public  Task<List<BarberSalon.Models.Category>> GetallCategory();
        public  Task ToogleStatus(int id);
        public Task<bool> existCategory(string name);
        public Task AddCategory(CreateCategoryBindingModel c);
        public  Task<BarberSalon.Models.Category> LoadCategoryById(int id);
        public  Task EditCategory(int id, CreateCategoryBindingModel s);
        public Task<List<BarberSalon.Models.Product>> GetAllProducts();
        public Task ToogleProductStatus(int id);
        public  Task<List<BarberSalon.Models.Category>> GetallCategoryActive();
        public  Task AddProduct(BarberSalon.Models.Product s);

        public  Task<BarberSalon.Models.Product> GetProductById(int id);






    }
}
