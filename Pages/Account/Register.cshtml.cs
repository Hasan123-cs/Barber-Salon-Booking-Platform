using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BarberSalon.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public IAccountServices _service;
        [BindProperty]
        public RegisterBindingViewModel bind { get; set; }
        public RegisterModel(IAccountServices service)
        {
            _service = service;
        }
        public void OnGet()

        {
        }
        public async Task<IActionResult> OnPost(bool IAgree )
        {

            Console.WriteLine("test value "+IAgree);
            if(IAgree == false)
            {
                ModelState.AddModelError("", "You must accept the terms");
            }
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _service.RegisterUser(bind);
            if(result.Count == 0)
            {
                return RedirectToPage("/Account/Login");

            }
            foreach (var error in result) {
                ModelState.AddModelError("", error);
            }
                return Page();
            
        }
    }
}
