using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Services;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberSalon.Pages.Admin.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public BarberSalon.Models.Product Product { get; set; } = new();

        [BindProperty]
        public IFormFile? ProductImage { get; set; }


        public List<BarberSalon.Models.Category> CategoryList { get; set; } = new();
        public IAdminService admin;
        public SupabaseService _supabase;
        public AppDbContext _db;
        public EditModel(IAdminService s,SupabaseService s1,AppDbContext ad)
        {
            _db = ad;
            admin = s;
            _supabase = s1;
        }
        public async Task<IActionResult> OnGet(int id )
        {
            CategoryList = await admin.GetallCategoryActive();
            Product = await admin.GetProductById(id);
            if(Product is null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine($"{Product.Name}");
            CategoryList = await admin.GetallCategoryActive();
            if(!ModelState.IsValid)
            {
                // debuging !! 
                //foreach (var item in ModelState)
                //{
                //    foreach (var error in item.Value.Errors)
                //    {
                //        Console.WriteLine($"{item.Key} => {error.ErrorMessage}");
                //    }
                //}
                return Page();
            }
            
            var p = await _db.Products.FindAsync(Product.Id);
            if(p is null)
            {
                return NotFound();
            }
            p.Name = Product.Name;
            p.Description = Product.Description;
            p.Price = Product.Price;
            p.StockStatus = Product.StockStatus;
            p.CategoryId = Product.CategoryId;
            p.IsActive = Product.IsActive;

            if (ProductImage != null)
            {
                p.ImageUrl = await _supabase.UploadImage(ProductImage);
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/Admin/Products/Index");
        }
    }
}
