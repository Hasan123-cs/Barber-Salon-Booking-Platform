using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Services
{
    public class DetailsModel : PageModel
    {
        public BarberSalon.Models.Service Service { get; set; }
        public ICustomerService service { get; set; }
        public DetailsModel(ICustomerService s)
        {
            service = s;
        }
        public async Task OnGet(int id)
        {
            Service = await service.LoadServiceById(id);
        }
    }
}
