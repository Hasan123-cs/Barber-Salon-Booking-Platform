using BarberSalon.Models.BindingModel;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Admin.Category
{
    [Authorize(Roles = "Admin")]

    public class EdittModel : PageModel
    {
        // create binding model because no need for new binding both share the common property needed
        [BindProperty]
        public CreateCategoryBindingModel Category { get; set; } = new();
        public IAdminService admin;
        [BindProperty]
        public int id { get; set; }
        public EdittModel(IAdminService s)
        {
            admin = s;
        }
        public async Task<IActionResult> OnGet(int ids )
        {
            id = ids;
            var c = await admin.LoadCategoryById(id);
            Console.WriteLine(id);
            if (c is null)
            {
                return RedirectToPage("/Admin/Category/Index");
            }
            Category = new CreateCategoryBindingModel()
            {
                Description = c.Description,
                IsActive = c.IsActive,
                Name = c.Name

            };
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            await admin.EditCategory(id, Category);
            return RedirectToPage("/Admin/Category/Index");
        }
    }
}
