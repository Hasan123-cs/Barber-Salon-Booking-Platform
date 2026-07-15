using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Home
{
    [Authorize(Roles = "Customer")]

    public class IndexModel : PageModel
    {
        public IEmployeeServices _service { get; set; }
        public List<BarberSalon.Models.Category> Categories { get; set; }
        public List<BarberSalon.Models.Employee> bestEmployee { get; set; } = new();
        public IndexModel(IEmployeeServices s)
        {
            _service = s;
        }
        public async Task OnGet()
        {
            bestEmployee = await _service.GetFirstThreeProfesionalEmployee();
            Categories = await _service.GetCategorys();
           
        }

    }
}
