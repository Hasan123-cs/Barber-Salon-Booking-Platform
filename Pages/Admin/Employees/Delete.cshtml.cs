using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Employees
{
    public class DeleteModel : PageModel
    {
        public IAdminService admin;
        public  UserManager<ApplicationUser> _userManager;
        public DeleteModel(IAdminService admin, UserManager<ApplicationUser> _userManager)
        {
            this._userManager = _userManager;
            this.admin = admin;
        }
        [BindProperty]
        public BarberSalon.Models.Employee Employee { get; set; }
        public async Task<IActionResult> OnGet(int id )
        {
            Employee = await admin.LoadEmployeeById(id);

            if (Employee == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var employee = await admin.LoadEmployeeById(Employee.Id);

            if (employee == null)   
            {

                return NotFound();
            }
            await admin.Diactive(employee.Id);
            if (!string.IsNullOrEmpty(employee.UserId))
            {
                var user = await _userManager.FindByIdAsync(employee.UserId);

                if (user != null)
                {
                    //diactive the account for 100 year
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

                    await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToPage("Dashboard");
        }
    }
}
