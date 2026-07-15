using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Pages.Admin.Schedule
{
    public class IndexModel : PageModel
    {
        public List<BarberSalon.Models.Employee> EmployeeList { get; set; } = new();
        public IAdminService admin;
        [BindProperty(SupportsGet = true)]
        public int EmployeeId { get; set; }
        [BindProperty]
        public List<BarberSalon.Models.WorkingHour> Schedule { get; set; } = new();
        public IndexModel(IAdminService s)
        {
            admin = s;
        }
        public async Task<IActionResult> OnGet()
        {
            EmployeeList = await admin.GetEmployeesInDB();
            if(EmployeeId == 0)
            {
                return Page();
            }
            Schedule = await admin.LoadSchedule(EmployeeId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("EmployeeId: " + EmployeeId);
            Console.WriteLine("Schedule Count: " + Schedule.Count);

            foreach (var item in Schedule)
            {
                Console.WriteLine($"{item.Day} {item.StartTime} {item.EndTime} {item.IsOffDay}");
            }
            EmployeeList = await admin.GetEmployeesInDB();
            if(!ModelState.IsValid)
            {
                return Page();
            }
            foreach (var item in Schedule)
            {
                // validate the dats
                if (!item.IsOffDay && item.StartTime >= item.EndTime)
                {
                    Console.WriteLine("hisssssssssssss");

                    ModelState.AddModelError("", $"{item.Day}: Start Time must be before End Time.");
                    return Page();
                }

                var existing = await admin.checkifExist(EmployeeId, item.Day);

                if (existing== null )
                {
                    Console.WriteLine("hisssss");

                    item.EmployeeId = EmployeeId;
                    await admin.AddWorkingHours(item);
                }
                else
                {
                    Console.WriteLine("hi");
                    await admin.UpdateWorkingHours(existing.Id, item);

                }
            }
            return RedirectToPage(new { EmployeeId });
        }
    }
}
