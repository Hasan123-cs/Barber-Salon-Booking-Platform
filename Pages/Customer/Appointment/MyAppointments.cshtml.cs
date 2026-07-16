using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer.Appointment
{
    public class MyAppointmentsModel : PageModel
    {
        public ICustomerService _custServ;
        public UserManager<ApplicationUser> _usermanager;
        public List<BarberSalon.Models.Appointment> Appointments { get; set; }
        public MyAppointmentsModel(ICustomerService customServ,UserManager<ApplicationUser> s)
        {
            _usermanager = s;
            _custServ = customServ;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await _usermanager.GetUserAsync(User);
            if(user is null)
            {
                return RedirectToPage("/Account/Login");
            }
            Appointments = await _custServ.GetAppointmentById(user.Id);
            return Page();
        }
        public async Task<IActionResult> OnPostCancel(int id )
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _custServ.CancelAppointment(id, user.Id);
            return RedirectToPage();
        }
    }
}
