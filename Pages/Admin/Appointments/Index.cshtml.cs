using BarberSalon.Models.Enum;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Appointments
{
    public class IndexModel : PageModel
    {
        public IAdminService admin;
        public List<BarberSalon.Models.Appointment> Appointments { get; set; }
        public IndexModel(IAdminService admin)
        {
            this.admin = admin;
        }
        public async Task OnGetAsync()
        {
            Appointments = await admin.LoadAppointments();
        }
        public async Task<IActionResult> OnPostAsync(int appointmentId,AppointmentStatus status)
        {
            var appointment = await admin.LoadAppointmentById(appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }


            await admin.UpdateAppointment(appointmentId, status);

            return RedirectToPage();
        }
    }
}
