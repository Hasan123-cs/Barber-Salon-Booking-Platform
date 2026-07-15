using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Category
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        public IAdminService admin;
        public List<BarberSalon.Models.Category> Categories { get; set; }
        public IndexModel(IAdminService admin)
        {
            this.admin = admin;
        }
        public async Task OnGet()
        {
            Categories = await admin.GetallCategory();
            
        }
        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            await admin.ToogleStatus(id);
            return Page();

        }
    }
}
