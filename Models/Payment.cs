using BarberSalon.Models.Enum;

namespace BarberSalon.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public StatusOfPayment PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public Order Order { get; set; }
    }
}
