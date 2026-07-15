using BarberSalon.Models;
using BarberSalon.Models.Enum;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Orders
{
    [Authorize(Roles = "Admin")]

    public class IndexModel : PageModel
    {
        public IAdminService admin;
        
        public List<Order> Orders { get; set; } = new List<Order>();
        public IndexModel(IAdminService admin)
        {
            this.admin = admin;
        }
        public async Task OnGet()
        {
            Orders = await admin.GetOrders();
        }
        public async Task<IActionResult> OnPostAsync(int orderId,OrderStatus status)
        {
            
            
          await  admin.updateOrder(orderId, status);
            TempData["Success"] = "Order updated successfully.";

            return RedirectToPage();
        }
    }
}
