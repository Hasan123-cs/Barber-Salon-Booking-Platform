namespace BarberSalon.Models
{
    // one small note that i consider isActive like IsDeleted
    public class Employee
    {
        
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Specialization { get; set; }

        public int ExperienceYears { get; set; }

        public string? ImageUrl { get; set; }


        public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<EmployeeService> EmployeeServices { get; set; }
            = new List<EmployeeService>();

        public ICollection<WorkingHour> WorkingHours { get; set; }
    = new List<WorkingHour>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
