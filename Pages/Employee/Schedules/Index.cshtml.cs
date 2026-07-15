using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Employee.Schedules;

[Authorize(Roles ="Employee")]
public class IndexModel : PageModel
{
    private readonly IAdminService _service;
    private readonly UserManager<ApplicationUser> _userManager;
    public IndexModel(IAdminService service,UserManager<ApplicationUser> userManager, IEmployeeServices emp)
    {
        _service = service;
        _userManager = userManager;
    }

    public List<WorkingHour> WorkingHours { get; set; } = new();

    public async Task OnGetAsync()
    {
            var user = await _userManager.GetUserAsync(User);

            WorkingHours = await _service.GetEmployeeSchedule(user.Id);
    }
}