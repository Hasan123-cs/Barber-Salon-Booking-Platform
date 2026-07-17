using BarberSalon.Models;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace BarberSalon.Pages.Admin.Dashboard
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        public UserManager<ApplicationUser> _UserManager;
        public string userName = string.Empty;
        // charts 
        public List<string> RevenueLabels { get; set; } = new();

        public List<decimal> RevenueData { get; set; } = new();
        public List<string> AppointmentLabels { get; set; } = new();

        public List<int> AppointmentData { get; set; } = new();
        public List<string> ServiceLabels { get; set; } = new();

        public List<int> ServiceData { get; set; } = new();
        public List<string> EmployeeLabels { get; set; } = new();

        public List<int> EmployeeData { get; set; } = new();
        // ==charts==

        public IAdminService admin;
        public int Customers { get; set; } = new();
        public int Employees { get; set; } = new();
        public int Orders { get; set; } = new();
        public decimal Revenue { get; set; } = new();

        public IndexModel(UserManager<ApplicationUser> userManager,IAdminService admin)
        {
            this.admin = admin;
            _UserManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var x = await _UserManager.GetUserAsync(User);
            if(x is null)
            {
                return;
            }
            userName = x.Email  ;
            var data  = await admin.GetMothlyRevenue();
            // get the month name and the total 
            RevenueLabels = data.Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)).ToList();
            RevenueData = data.Select(x => x.Revenue).ToList();
            var appointmentStatus = await admin.GetAppointmentStatusCount();

            AppointmentLabels = appointmentStatus.Select(x => x.Status.ToString()).ToList();

            AppointmentData = appointmentStatus.Select(x => x.Count).ToList();
           
            var services = await admin.GetMostBookedServices();

            ServiceLabels = services.Select(x => x.ServiceName).ToList();
            
            ServiceData = services.Select(x => x.BookingCount).ToList();

            var employees = await admin.GetTopEmployees();

            EmployeeLabels = employees.Select(x => x.EmployeeName).ToList();

            EmployeeData = employees.Select(x => x.BookingCount).ToList();
            // get data status 
            var FullData = await admin.GetAllData();
            Customers = FullData.customerCount;
            Employees= FullData.employeeCount;
            Orders = FullData.orderCount;
            Revenue = FullData.revenue;
        }
    }
}
