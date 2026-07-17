using BarberSalon.DTO;
using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Models.Enum;

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
        public  Task<List<Employee>> GetEmployeesInDB();

        public Task<List<WorkingHour>> LoadSchedule(int eid);
        public  Task<WorkingHour> checkifExist(int EmployeeId, DayOfWeek day);
        public  Task AddWorkingHours(WorkingHour d);
        public Task UpdateWorkingHours(int id, WorkingHour s1);
        public  Task<List<BarberSalon.Models.Appointment>> LoadAppointments();
        public  Task<BarberSalon.Models.Appointment> LoadAppointmentById(int id);

        public  Task UpdateAppointment(int id, AppointmentStatus status);
        public  Task AddEmployee(BarberSalon.Models.Employee e);


        public  Task<BarberSalon.Models.Employee> LoadEmployeeById(int id);
        public  Task Diactive(int id);


        public Task<List<BarberSalon.Models.Employee>> LoadActiveEmployee();
        public Task<List<BarberSalon.Models.Service>> GetAllService();
        public Task<BarberSalon.Models.Service> GetServiceById(int id);
        public  Task DeleteService(int id);

        public  Task AddService(BarberSalon.Models.Service s);

        public  Task updateService(BarberSalon.Models.Service s);
        public  Task<List<MonthlyRevenueDto>> GetMothlyRevenue();



        public Task<List<BarberSalon.Models.Order>> GetOrders();

        public  Task<List<AppointmentStatusVM>> GetAppointmentStatusCount();

        public  Task<List<ServiceBookingVM>> GetMostBookedServices();

        public  Task<List<EmployeeBookingVM>> GetTopEmployees();

        public Task<(int customerCount, int employeeCount, int orderCount, decimal revenue)> GetAllData();

        public Task updateOrder(int id, OrderStatus status);


    }
}
