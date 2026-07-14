using BarberSalon.Services.Implements;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Category
{
    public class IndexModel : PageModel
    {
        public BarberSalon.Models.Category Category { get; set; }
        public List<BarberSalon.Models.Product> Products { get; set; } = new();
        public ICustomerService _service;
        public IndexModel(ICustomerService s)
        {
            _service = s;
        }
        public async Task OnGet(int categoryId)
        {
            Category = await _service.GetCategoryById(categoryId)
               ;
            Products = await _service.GetProductsByCategoryId(categoryId);
        }
    }
}
