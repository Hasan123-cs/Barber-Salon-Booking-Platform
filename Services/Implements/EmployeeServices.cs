using BarberSalon.Data;
using Microsoft.EntityFrameworkCore;
using BarberSalon.Services.Interfaces;
namespace BarberSalon.Services.Implements
{
    public class EmployeeServices : IEmployeeServices
    {
        public AppDbContext _db;
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
