using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services.Implements
{
    public class CustomerService : ICustomerService
    {
        public AppDbContext _db;
        public CustomerService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Service>> GetAllServices()
        {
            return await _db.Services.ToListAsync();
        }
        public async Task<BarberSalon.Models.Service> LoadServiceById(int id )
        {
            return await _db.Services.FindAsync(id);
        }
    }
}
