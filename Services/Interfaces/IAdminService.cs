using BarberSalon.Models;

namespace BarberSalon.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<WorkingHour>> GetEmployeeSchedule(string employeeId);
        public Task<string> GetEmployeeName(string employeeId);
    }
}
