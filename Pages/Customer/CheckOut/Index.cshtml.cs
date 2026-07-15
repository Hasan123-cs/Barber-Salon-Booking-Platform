using BarberSalon.Models;
using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace BarberSalon.Pages.Customer.CheckOut
{

    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public string SelectedPayment { get; set; }
        public ICustomerService c;

        public IndexModel(UserManager<ApplicationUser> userManager, ICustomerService x)
        {
            _userManager = userManager;
            c = x;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            // second way of access session from the backend (prefer this for validation and testing)
            var json = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("/Customer/Cart/Index");
            }
            // deserialize the json file into a list of cart item class 
            var cart =JsonSerializer.Deserialize<List<CartItem>>(json);
            var user =await _userManager.GetUserAsync(User);
            if (cart is null || user == null)
            {
                return RedirectToPage("/Customer/Home/Index");
            }
            bool result = await c.MakeTheOrder(user,cart,SelectedPayment);
            // after make the order if all ok remove the session other way error occur can view in terminal..
            if (result)
            {
                // clear the session 
                HttpContext.Session.Remove("Cart");
                return RedirectToPage("/Customer/Success", new {key="order"});
            }
            else
            {

            return BadRequest("Cannot Make the order");
            }

        }
    }
}
