using BarberSalon.Data;
using Microsoft.EntityFrameworkCore;
using BarberSalon.Services.Interfaces;
using BarberSalon.Models;
namespace BarberSalon.Services.Implements
{
    public class EmployeeServices : IEmployeeServices
    {
        public AppDbContext _db;

        public async Task<List<EmployeeService>> GetAllEmployee(int serviceID) { 
            var result = await _db.EmployeeServices.Where(x=>x.ServiceId == serviceID).Include(x => x.Employee).ToListAsync();
            return result; 
        }

        public EmployeeServices(AppDbContext _db)
        {
            this._db = _db;
        }
        public async Task<List<BarberSalon.Models.Employee>> GetFirstThreeProfesionalEmployee()
        {
            var result = await _db.Employees.Where(x => x.IsActive).OrderByDescending(e => e.ExperienceYears)
                .Take(3).ToListAsync();
            return result;
        }
        public async Task<List<BarberSalon.Models.Category>> GetCategorys()
        {
           var  Categories = await _db.Categories
             .Where(c => c.IsActive)
             .ToListAsync();
            return Categories;
        }
    }
}
