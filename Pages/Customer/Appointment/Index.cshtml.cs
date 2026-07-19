using BarberSalon.Models;
using BarberSalon.Models.Enum;
using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarberSalon.Pages.Customer.Appointment
{
    public class IndexModel : PageModel
    {
        public UserManager<ApplicationUser> _userManager;
        public IEmployeeServices _service;
        public ICustomerService _custService;

        public IndexModel(UserManager<ApplicationUser> userManager, ICustomerService custService,IEmployeeServices _s)
        {
            _service = _s;
            _userManager = userManager;
            _custService = custService;
        }
        public BarberSalon.Models.Service Service { get; set; }

        [BindProperty]
        public BarberSalon.Models.Appointment Appointment { get; set; }

        [BindProperty]
        public DateTime SelectedDate { get; set; } = DateTime.Now;

        [BindProperty]
        public string SelectedTime { get; set; }

        public List<BarberSalon.Models.EmployeeService> EmployeeList { get; set; } = new();

        public List<string> AvailableTimes { get; set; } = new();
        public async Task OnGet(int serviceId)
        {
            Service = await _custService.LoadServiceById(serviceId);
            EmployeeList = await _service.GetAllEmployee(serviceId);
            Console.WriteLine("Employees count: " + EmployeeList.Count);

            Appointment = new BarberSalon.Models.Appointment(){ServiceId = serviceId};
        }
        public async Task<IActionResult> OnPostLoadTimes()
        {
            
            Service = await _custService.LoadServiceById(Appointment.ServiceId);
            SelectedDate = SelectedDate;

            EmployeeList = await _service.GetAllEmployee(Appointment.ServiceId);
            // here we can use it in client side using min attribute but in client bypass since the attacker can enter devtools and 
            // use paste date then send normaly to server 
            if (SelectedDate.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("", "Cannot select a past date");
                return Page();
            }
            AvailableTimes = await _custService.GetAvailableTimes(Appointment.EmployeeId,Appointment.ServiceId,SelectedDate);


            return Page();
        }
        public async Task<IActionResult> OnPostBook()
        {
            Service = await _custService.LoadServiceById(Appointment.ServiceId);
            SelectedDate = SelectedDate;

            EmployeeList = await _service.GetAllEmployee(Appointment.ServiceId);
            AvailableTimes = await _custService.GetAvailableTimes(Appointment.EmployeeId, Appointment.ServiceId, SelectedDate);


            foreach (var x in AvailableTimes)
            {
                Console.WriteLine("time " + x);
            }

            if (!AvailableTimes.Contains(SelectedTime))
            {
                ModelState.AddModelError("", "This time is no longer available");

                return Page();
            }
            // to convert the time 
            TimeOnly start = TimeOnly.Parse(SelectedTime);
            TimeOnly end = start.AddMinutes(Service.Duration);
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Appointment.UserId = user.Id;

            Appointment.AppointmentDate = SelectedDate.Date;

            Appointment.StartTime = start;

            Appointment.EndTime = end;

            Appointment.CreatedAt = DateTime.UtcNow;

            Appointment.Status = AppointmentStatus.Pending;
            await _custService.BookAppointment(Appointment);
           

            return RedirectToPage("/Customer/Success", new {key= "appointment" });
           
        }
    }
}
