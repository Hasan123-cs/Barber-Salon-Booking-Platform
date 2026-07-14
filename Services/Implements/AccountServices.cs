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
        public UserManager<ApplicationUser> _usermanager;
        public AccountServices(AppDbContext db, SignInManager<ApplicationUser> manager, UserManager<ApplicationUser> usermanager)
        {
            _db = db;
            _manager = manager;
            _usermanager = usermanager;
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
        public async Task<List<string>> RegisterUser(RegisterBindingViewModel b)
        {
            var user = new ApplicationUser()
            {
                UserName = b.Email,
                Email = b.Email,
                PhoneNumber=b.PhoneNumber,
                FullName=b.FullName,

            };
            var result = await _usermanager.CreateAsync(user, b.Password);
            if(result.Succeeded)
            {
                return [];
            }
            return result.Errors.Where(r => r.Code!="DuplicateUserName").Select(x => x.Description).ToList(); 
        }
    }
}
