using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services.Implements
{
    public class AdminService : IAdminService
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
        public async Task<List<BarberSalon.Models.Category>> GetallCategory()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task ToogleStatus(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
            {
                return;
            }

            category.IsActive = !category.IsActive;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> existCategory(string name)
        {
            var exist = await _db.Categories.AnyAsync(x => x.Name == name);
            return exist;

        }
        public async Task AddCategory(CreateCategoryBindingModel c)
        {
            var category = new BarberSalon.Models.Category()
            {
                Description = c.Description,
                IsActive = c.IsActive,
                Name = c.Name,
            };
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }
        public async Task<BarberSalon.Models.Category> LoadCategoryById(int id)
        {
            return await _db.Categories.FindAsync(id);
        }
        public async Task EditCategory(int id,CreateCategoryBindingModel s)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category is null)
            {
                return;
            }
            category.Description = s.Description;
            category.IsActive=s.IsActive;
            category.Name=s.Name;
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Product>> GetAllProducts()
        {
            return await _db.Products.ToListAsync();
        }
        public async Task ToogleProductStatus(int id)
        {
            var pr = await _db.Products.FindAsync(id);
            if (pr is null)
            {
                return;
            }
            pr.IsActive = !pr.IsActive;
            // we dont add AddASync because EF do the tracking this one of most EF Characteristic Important :)
            await _db.SaveChangesAsync();
        }
        public async Task<List<BarberSalon.Models.Category>> GetallCategoryActive()
        {
            return await _db.Categories.Where(x=>x.IsActive==true).ToListAsync();
        }
        public async Task AddProduct(BarberSalon.Models.Product s)
        {
            await _db.Products.AddAsync(s);
            await _db.SaveChangesAsync();
        }
        public async Task<BarberSalon.Models.Product> GetProductById(int id )
        {
            return await _db.Products.FindAsync(id);
        }
    }
}
