using BarberSalon.Services;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Service
{
    public class CreateModel : PageModel
    {
        public SupabaseService _servicesupa;
        public IAdminService admin;
        public CreateModel(SupabaseService service,IAdminService s)
        {
            admin = s;
            _servicesupa = service;
        }
        [BindProperty]
        public Models.Service Service { get; set; } = new();

        [BindProperty]
        public IFormFile? ServiceImage { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (ServiceImage != null)
            {
                Service.ImageUrl =  await _servicesupa.UploadImage(ServiceImage);
            }

           await admin.AddService(Service);
            TempData["Success"] = "Service created successfully.";

            return RedirectToPage("Index");
        }
    }
}
