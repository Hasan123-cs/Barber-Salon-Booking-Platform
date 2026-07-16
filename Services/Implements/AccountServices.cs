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
            var result = await _manager.PasswordSignInAsync(l.email, l.password, false, false);
            if (result.Succeeded)
            {
                var user = await _manager.UserManager.FindByEmailAsync(l.email);
                if(user is null)
                {
                    return -1;
                }
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
                PhoneNumber = b.PhoneNumber,
                FullName = b.FullName,

            };
            var result = await _usermanager.CreateAsync(user, b.Password);
            if (result.Succeeded)
            {
                 await _usermanager.AddToRoleAsync(user, "Customer");
                return [];
            }
            return result.Errors.Where(r => r.Code != "DuplicateUserName").Select(x => x.Description).ToList();
        }
        public async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Employee", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            string emailAdmin = "admin@example.com";
            string emailEmployees = "employee@example.com";
            string emailCustomer = "customer@example.com";

            // Siding Data For Testing 

            // employee 
            var employee = await _usermanager.FindByEmailAsync(emailEmployees);

            if (employee == null)
            {
                employee = new ApplicationUser
                {
                    FullName = "Hair Cut Test Employee",
                    UserName = emailEmployees,
                    Email = emailEmployees,
                    PhoneNumber = "70111222",
                    EmailConfirmed = true
                };

                var result = await _usermanager.CreateAsync(employee, "Employee123");

                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(employee, "Employee");

                    _db.Employees.Add(new Employee
                    {
                        UserId = employee.Id,
                        Name = employee.FullName,
                        Phone = employee.PhoneNumber!,
                        Specialization = "Hair Cut",
                        ExperienceYears = 5,
                        ImageUrl = null,
                        IsActive = true
                    });

                    await _db.SaveChangesAsync();
                }
            }
            // customer 
            var customer = await _usermanager.FindByEmailAsync(emailCustomer);

            if (customer == null)
            {
                customer = new ApplicationUser
                {
                    FullName = "Test Customer",
                    UserName = emailCustomer,
                    Email = emailCustomer,
                    PhoneNumber = "71122334",
                    EmailConfirmed = true
                };

                var result = await _usermanager.CreateAsync(customer, "Customer123");

                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(customer, "Customer");
                }
            }
            // admin 
            var admin = await _usermanager.FindByEmailAsync(emailAdmin);


            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FullName = "System Admin",
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    // its mean trust the email imagine as confirmation via gmail 
                    EmailConfirmed = true
                };

                var result = await _usermanager.CreateAsync(admin, "Admin123");

                if (result.Succeeded)
                {
                    await _usermanager.AddToRoleAsync(admin, "Admin");
                }
            }
            // === Siding Data For Testings ===


        }
    }
}
