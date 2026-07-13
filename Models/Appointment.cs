using BarberSalon.Models.Enum;

namespace BarberSalon.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int EmployeeId { get; set; }

        public int ServiceId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }

        public Employee Employee { get; set; }

        public Service Service { get; set; }
    }
}
