using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Category
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateCategoryBindingModel Category { get; set; } = new();
        public IAdminService admin;
        public CreateModel(IAdminService s)
        {
            admin = s;
        }
        public void OnGet()
        {
            Category.IsActive = true;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            bool exist = await admin.existCategory(Category.Name);
            if(exist)
            {
                ModelState.AddModelError("", "Category Already Exist in Our Store");
                return Page();
            }
            await    admin.AddCategory(Category);
            return RedirectToPage("/Admin/Category/Index");
        }
    }
}
