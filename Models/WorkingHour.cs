using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BarberSalon.Models
    {
    public class WorkingHour
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public bool IsOffDay { get; set; }
        [ValidateNever]
        public Employee? Employee { get; set; }
    }
}
