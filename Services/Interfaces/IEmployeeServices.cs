using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Models.Enum;
using System.Security.Claims;

namespace BarberSalon.Services.Interfaces
{
    public interface IEmployeeServices
    {
        public  Task<List<BarberSalon.Models.Employee>> GetFirstThreeProfesionalEmployee();
        public  Task<List<BarberSalon.Models.Category>> GetCategorys();
        public Task<List<BarberSalon.Models.EmployeeService>> GetAllEmployee(int serviceID);
        Task<EmployeeDashboardVM> GetDashboard(ClaimsPrincipal user);
        Task<List<Appointment>> GetMyAppointments(string userId);
        public  Task<BarberSalon.Models.Appointment> GetAppointmentById(int id, string userId);
        public Task UpdateStatusOfAppointmentEmployee(int id, string userId, AppointmentStatus status);
        public  Task<string> getUserId(int eid);



    }
}
