using BarberSalon.Data;
using BarberSalon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System.Text.Json;

namespace BarberSalon.Pages.Customer.Cart
{

    public class IndexModel : PageModel
    {
        public List<CartItem> CartItems { get; set; } = new();
        // property

        public decimal SubTotal => CartItems.Sum(x => x.Price * x.Quantity);
        

        public void OnGet()
        {
            var json = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(json))
            {
                CartItems = JsonSerializer.Deserialize<List<CartItem>>(json)!;
            }
        }

        public async Task<IActionResult> OnPostRemove(int productId)
        {
            var json = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(json))
            {
                CartItems = JsonSerializer.Deserialize<List<CartItem>>(json)!;

                var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

                if (item != null)
                {
                    CartItems.Remove(item);
                }

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(CartItems));
            }

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int productId, int quantity)
        {
            var json = HttpContext.Session.GetString("Cart");

            if (!string.IsNullOrEmpty(json))
            {
                CartItems = JsonSerializer.Deserialize<List<CartItem>>(json)!;

                var item = CartItems.FirstOrDefault(x => x.ProductId == productId);

                if (item != null)
                {
                    item.Quantity = quantity < 1 ? 1 : quantity;
                }

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(CartItems));
            }

            return RedirectToPage();
        }

     
    }
}
