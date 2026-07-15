using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Employees
{
    public class DashboardModel : PageModel
    {
        public List<BarberSalon.Models.Employee> Employees { get; set; }
        public IAdminService admin;
        public DashboardModel(IAdminService admin)
        {
            this.admin = admin;
        }

        public async Task OnGet()
        {
            Employees = await admin.LoadActiveEmployee();
        }
    }
}
