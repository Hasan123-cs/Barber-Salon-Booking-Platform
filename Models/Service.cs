namespace BarberSalon.Models
{
    public class Service
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }


        public ICollection<EmployeeService> EmployeeServices { get; set; }
            = new List<EmployeeService>();

        public ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();
    }
}
