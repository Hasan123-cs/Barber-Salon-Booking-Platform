using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Order
{

    public class OrderDetailsModel : PageModel
    {
        public ICustomerService cust;
        public UserManager<ApplicationUser> _usermagr;
        public BarberSalon.Models.Order Order { get; set; }

        public OrderDetailsModel(ICustomerService cust, UserManager<ApplicationUser> m)
        {
            _usermagr = m;
            this.cust = cust; 
        }
        public async Task OnGet(int id )
        {
            var user = await _usermagr.GetUserAsync(User);
            if(user is null)
            {
                return;
            }
            Order = await cust.LoadOrderById(id, user.Id);

        }
    }
}
