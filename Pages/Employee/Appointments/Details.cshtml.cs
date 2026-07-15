using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Employee.Appointments
{
    // Role Access Based 
    [Authorize(Roles ="Employee")]
    public class DetailsModel : PageModel
    {
        public IEmployeeServices _serv;
        public UserManager<ApplicationUser> _userManager;
        public DetailsModel(IEmployeeServices serv, UserManager<ApplicationUser> s)
        {
            _userManager = s;
            _serv = serv;
        }
        [BindProperty]
        public BarberSalon.Models.Appointment Appointment { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user is null)
            {
                return RedirectToPage("/Account/Login");
            }
            Appointment = await _serv.GetAppointmentById(id, user.Id);
            if (Appointment == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user is null)
            {
                return RedirectToPage("/Account/Login");

            }
            await _serv.UpdateStatusOfAppointmentEmployee(Appointment.Id, user.Id, Appointment.Status);
            return RedirectToPage("/Employee/Appointments/Index");
        }

    }
}
