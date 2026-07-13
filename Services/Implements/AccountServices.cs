using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Interfaces;
using BarberSalon.Data;
using Microsoft.AspNetCore.Identity;
using BarberSalon.Models;
namespace BarberSalon.Services.Implements
{
    public class AccountServices : IAccountServices
    {
        public AppDbContext _db;
        public SignInManager<ApplicationUser> _manager;
        public AccountServices(AppDbContext db, SignInManager<ApplicationUser> manager)
        {
            _db = db;
            _manager = manager;
        }
        public async Task<int> LoginUser(LoginBindingModel l)
        {
            var result = await _manager.PasswordSignInAsync(l.email, l.password,false,false);
            if(result.Succeeded)
            {
                var user = await _manager.UserManager.FindByEmailAsync(l.email);

                if (await _manager.UserManager.IsInRoleAsync(user, "Admin"))
                {
                    return 1;
                }

                if (await _manager.UserManager.IsInRoleAsync(user, "Employee"))
                {
                    return 2;
                }

                return 3;
            }
            return -1;
           
        }
    }
}
