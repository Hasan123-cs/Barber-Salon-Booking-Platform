using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Order
{

    public class IndexModel : PageModel
    {
        [BindProperty]
        public BarberSalon.Models.Product Product { get; set; } = new();
        public ICustomerService _ser;
        [BindProperty]
        public int Quantity { get; set; }
        public IndexModel(ICustomerService s)
        {
            _ser = s;
        }

        public async Task OnGet(int id )
        {
            Product = await _ser.getProductById(id);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _ser.AddToCart(Product.Id, Quantity);
            return RedirectToPage("/Category/Index", new { categoryId = Product.CategoryId });
        }
    }
}
