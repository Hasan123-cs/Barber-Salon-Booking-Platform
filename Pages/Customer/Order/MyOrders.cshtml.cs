using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Order
{

    public class MyOrdersModel : PageModel
    {

        public UserManager<ApplicationUser> _userManager;
        public List<BarberSalon.Models.Order> Orders { get; set; } = new();
        public ICustomerService cust;


        public MyOrdersModel(UserManager<ApplicationUser> userManager,ICustomerService s)
        {
            cust = s;
            _userManager = userManager;
        }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user is null)
            {
                return;
            }
            Orders = await cust.GetAllOrderByUser(user.Id);
        }
    }
}
