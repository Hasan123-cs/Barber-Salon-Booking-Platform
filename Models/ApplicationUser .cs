namespace BarberSalon.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Employee? Employee { get; set; }
    }
}
