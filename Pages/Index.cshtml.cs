using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            PasswordHasher<string> s=new PasswordHasher<string>();
            Console.WriteLine(s.HashPassword(null, "test123"));
           return RedirectToPage("/Customer/Home/Index");
        }
    }
}
