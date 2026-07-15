using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services.Implements
{
    public class AdminService :IAdminService
    {
        public AppDbContext _db;
        public AdminService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkingHour>> GetEmployeeSchedule(string employeeId)
        {
            return await _db.WorkingHours.Include(w => w.Employee).Where(w => w.Employee.UserId == employeeId).OrderBy(w => w.Day).ToListAsync();
        }
        public async Task<string> GetEmployeeName(string employeeId)
        {
            return await _db.Employees.Select(e => e.UserId).FirstAsync();
        }

       
    }
}
