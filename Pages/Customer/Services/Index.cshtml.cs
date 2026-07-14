using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Services
{
    public class IndexModel : PageModel
    {
        public List<BarberSalon.Models.Service> Services { get; set; } = new();
        public ICustomerService c;
        public IndexModel(ICustomerService c)
        {
            this.c = c;
        }

        public async Task OnGet()
        {
            Services = await c.GetAllServices();
        }
    }
}
