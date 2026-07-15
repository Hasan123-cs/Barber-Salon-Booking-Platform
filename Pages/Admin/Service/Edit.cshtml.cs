using BarberSalon.Services;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Service
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        public SupabaseService supabase;
        public IAdminService admin;
        public EditModel(SupabaseService supabase, IAdminService admin)
        {
            this.supabase = supabase;
            this.admin = admin;
        }
        [BindProperty]
        public BarberSalon.Models.Service Service { get; set; } = new();
        [BindProperty]
        public IFormFile? ServiceImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Service = await admin.GetServiceById(id);
            if (Service == null)
            {

                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                return Page();
            }

            var service = await admin.GetServiceById(Service.Id);

            if (service == null)
            {

                return NotFound();
            }

            service.Name = Service.Name;
            service.Description = Service.Description;
            service.Price = Service.Price;
            service.Duration = Service.Duration;
            service.IsActive = Service.IsActive;

            if (ServiceImage != null)
            {
                service.ImageUrl = await supabase.UploadImage(ServiceImage);
            }

            await admin.updateService(service);

            TempData["Success"] = "Service updated successfully.";

            return RedirectToPage("Index");
        }
    }
}
