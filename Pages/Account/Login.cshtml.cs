using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginBindingModel _login { get; set; } = new();
        public IAccountServices _service { get; set; }
        public LoginModel(IAccountServices s)
        {
            _service = s;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var usercheck = await _service.LoginUser(_login);
            if (usercheck == 1)
            {
                return RedirectToPage("/Admin/Dashboard");

            }
            else if (usercheck == 2)
            {
                return RedirectToPage("/Employee/Dashboard");

            }
            else if (usercheck == 3)
            {
                return RedirectToPage("/Customer/Index");
            }
            else
            {
                ModelState.AddModelError("","Invalid Email Or Password");
                 
                return Page();
            }
        }
    }
}
