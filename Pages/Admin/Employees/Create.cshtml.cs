using BarberSalon.Models;
using BarberSalon.Models.BindingModel;
using BarberSalon.Services;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Employees
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateEmployeeBindingModel Input { get; set; } = new();

        [BindProperty]
        public IFormFile? EmployeeImage { get; set; }
        public  UserManager<ApplicationUser> _userManager;
        public  SupabaseService _storageService;
        public IAdminService admin;
        public CreateModel( UserManager<ApplicationUser> userManager, SupabaseService storageService, IAdminService s)
        {
            admin = s;
            _userManager = userManager;
            _storageService = storageService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // email exist ? 
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return Page();
            }

            string? imageUrl = null;

            if (EmployeeImage != null)
            {
                imageUrl = await _storageService.UploadImage(EmployeeImage);
            }

            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = Input.FullName,
                PhoneNumber = Input.Phone
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }

            // Add the employee role
            await _userManager.AddToRoleAsync(user, "Employee");

            var employee = new BarberSalon.Models.Employee
            {
                UserId = user.Id,
                Name = Input.Name,
                Phone = Input.Phone,
                Specialization = Input.Specialization,
                ExperienceYears = Input.ExperienceYears,
                ImageUrl = imageUrl,
                IsActive = Input.IsActive
            };
            await admin.AddEmployee(employee);


            return RedirectToPage("Dashboard");
        }
    }
}
