namespace BarberSalon.Models.BindingModel
{
    public class EmployeeDashboardVM
    {
        public int TodayCount { get; set; }

        public int PendingCount { get; set; }

        public int CompletedCount { get; set; }

        public int CancelledCount { get; set; }

        public List<Appointment> TodayAppointments { get; set; } = new();
    }
}
