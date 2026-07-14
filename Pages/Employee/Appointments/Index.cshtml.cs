using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Employee.Appointments
{
    [Authorize(Roles = "Employee")]
    public class IndexModel : PageModel
    {
        private readonly IEmployeeServices _employeeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IEmployeeServices employeeService, UserManager<ApplicationUser> userManager)
        {
            _employeeService = employeeService;
            _userManager = userManager;
        }

        public List<Appointment> Appointments { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return;
            }
            Appointments = await _employeeService.GetMyAppointments(userId);
        }
    }
    }
