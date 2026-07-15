using BarberSalon.Services;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Products
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public BarberSalon.Models.Product Product { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        private readonly SupabaseService _supabase;
        public List<BarberSalon.Models.Category> Categories { get; set; }
        public IAdminService admin { get; set; }
        public CreateModel(IAdminService s)
        {
            admin = s;
        }
        public async Task OnGet()
        {
            Categories = await admin.GetallCategoryActive();
            Product.IsActive = true;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
            Categories = await admin.GetallCategoryActive();
            if (!ModelState.IsValid)
            {

                return Page();
            }
            string url = await _supabase.UploadImage(ImageFile);
            Product.ImageUrl = url;
            await admin.AddProduct(Product);
            return RedirectToPage("/Admin/Products/Index");

        }
    }
}
