using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {

        public List<BarberSalon.Models.Product> Products { get; set; } = new();
        public IAdminService admin;
        public IndexModel(IAdminService s)
        {
            admin = s;
        }

        public async Task OnGet()
        {
            Products = await admin.GetAllProducts();
        }
        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            await admin.ToogleProductStatus(id);
            // reload the page
            return RedirectToPage();
        }
    }
}
