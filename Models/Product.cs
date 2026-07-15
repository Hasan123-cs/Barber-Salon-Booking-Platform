using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BarberSalon.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockStatus { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category? Category { get; set; }
        public bool IsActive { get; set; }
        [ValidateNever]

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
