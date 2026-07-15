using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Models.Enum;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
namespace BarberSalon.Services.Implements
{
    public class EmployeeServices : IEmployeeServices
    {
        public AppDbContext _db;
        public UserManager<ApplicationUser> _userManager;
        public async Task<List<EmployeeService>> GetAllEmployee(int serviceID)
        {
            var result = await _db.EmployeeServices.Where(x => x.ServiceId == serviceID).Include(x => x.Employee).ToListAsync();
            return result;
        }

        public EmployeeServices(AppDbContext _db, UserManager<ApplicationUser> userManager)
        {
            this._db = _db;
            _userManager = userManager;
        }
        public async Task<List<BarberSalon.Models.Employee>> GetFirstThreeProfesionalEmployee()
        {
            var result = await _db.Employees.Where(x => x.IsActive).OrderByDescending(e => e.ExperienceYears)
                .Take(3).ToListAsync();
            return result;
        }
        public async Task<List<BarberSalon.Models.Category>> GetCategorys()
        {
            var Categories = await _db.Categories
              .Where(c => c.IsActive)
              .ToListAsync();
            return Categories;
        }
        public async Task<EmployeeDashboardVM> GetDashboard(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);

            var employee = await _db.Employees.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (employee == null)
            {
                return new EmployeeDashboardVM();
            }

            DateTime today = DateTime.UtcNow.Date;

            var appointments = await _db.Appointments
                .Include(x => x.User)
                .Include(x => x.Service)
                .Where(x =>
                    x.EmployeeId == employee.Id &&
                    x.AppointmentDate.Date == today)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            return new EmployeeDashboardVM
            {
                TodayAppointments = appointments,

                TodayCount = appointments.Count,

                PendingCount = appointments.Count(x =>
                    x.Status == AppointmentStatus.Pending),

                CompletedCount = appointments.Count(x =>
                    x.Status == AppointmentStatus.Completed),

                CancelledCount = appointments.Count(x =>
                    x.Status == AppointmentStatus.Cancelled)
            };
        }
        public async Task<List<Appointment>> GetMyAppointments(string userId)
        {
            var employee = await _db.Employees.FirstOrDefaultAsync(x => x.UserId == userId);

            if (employee == null)
            {
                return new List<Appointment>();
            }

            return await _db.Appointments
                .Include(x => x.User)
                .Include(x => x.Service)
                .Where(x => x.EmployeeId == employee.Id)
                .OrderBy(x => x.AppointmentDate)
                .ThenBy(x => x.StartTime)
                .ToListAsync();


        }
        //  appointment schedule
        public async Task<BarberSalon.Models.Appointment> GetAppointmentById(int id , string userId)
        {

           return await _db.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a =>a.Id == id &&a.Employee.UserId == userId);
        }
        public async Task UpdateStatusOfAppointmentEmployee(int id , string userId,AppointmentStatus status)
        {
            var appointment = await _db.Appointments.Include(a => a.Employee).FirstOrDefaultAsync(a =>a.Id == id && a.Employee.UserId == userId);
            if (appointment == null)
            {
                return ;
            }
            appointment.Status = status;
            await _db.SaveChangesAsync();
        }
        public async Task<string> getUserId(int eid)
        {
            return await _db.Employees.Where(x => x.Id == eid).Select(x=>x.UserId).FirstAsync();
        }

    }
}
