using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Employee.Dashboard
{
    [Authorize(Roles ="Employee")]
    public class IndexModel : PageModel
    {
        private readonly IEmployeeServices _employeeService;

        public IndexModel(IEmployeeServices employeeService)
        {
            _employeeService = employeeService;
        }

        public int TodayCount { get; set; }

        public int PendingCount { get; set; }

        public int CompletedCount { get; set; }

        public int CancelledCount { get; set; }

        public List<Appointment> TodayAppointments { get; set; } = new();

        public async Task OnGet()
        {
            var dashboard = await _employeeService.GetDashboard(User);

            TodayCount = dashboard.TodayCount;
            PendingCount = dashboard.PendingCount;
            CompletedCount = dashboard.CompletedCount;
            CancelledCount = dashboard.CancelledCount;

            TodayAppointments = dashboard.TodayAppointments;
        }
        
    }
}
