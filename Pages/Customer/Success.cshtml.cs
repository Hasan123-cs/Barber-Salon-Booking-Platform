using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Customer
{
    public class SuccessModel : PageModel
    {
        // to get it from the route parameter 
        [BindProperty(SupportsGet = true)]
        public string Key { get; set; } = "";
        public string Title { get; set; } = "";
        public string Message { get; set; } = "";
        public string ButtonText { get; set; } = "Continue";
        public string ButtonUrl { get; set; } = "/";

        public void OnGet()
        {
            switch (Key?.ToLower())
            {
                case "order":
                    Title = "Order Placed Successfully!";
                    Message = "Thank you for your purchase. Your order has been received and is now being processed.";
                    ButtonText = "View Orders";
                    ButtonUrl = "/Customer/Order/MyOrders";
                    break;

                case "appointment":
                    Title = "Appointment Booked!";
                    Message = "Your appointment has been booked successfully.";
                    ButtonText = "My Appointments";
                    ButtonUrl = "/Customer/Appointment/MyAppointments";
                    break;

                

                default:
                    Title = "Success!";
                    Message = "Your request has been completed successfully.";
                    ButtonText = "Back Home";
                    ButtonUrl = "/";
                    break;
            }
        }
    }
}
