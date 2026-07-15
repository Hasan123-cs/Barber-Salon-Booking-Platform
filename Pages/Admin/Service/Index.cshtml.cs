using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Service
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IList<BarberSalon.Models.Service> Services { get; set; } = new List<BarberSalon.Models.Service>();
        public IAdminService admin;
        public IndexModel( IAdminService admin)
        {
            this.admin = admin;
        }

        public async Task OnGet()
        {
          Services =  await admin.GetAllService();

        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            Console.WriteLine(id);
            var service =await  admin.GetServiceById(id);

            if (service == null)
            {
                return NotFound();
            }

            await admin.DeleteService(service.Id);
            
            TempData["Success"] = "Service removed successfully.";

            return RedirectToPage();
        }
    }
}
