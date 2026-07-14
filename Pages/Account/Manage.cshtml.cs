using BarberSalon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarberSalon.Pages.Account
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [BindProperty]
        public ApplicationUser UserProfile { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            UserProfile.FullName = user.FullName;
            UserProfile.Email = user.Email;
            UserProfile.PhoneNumber = user.PhoneNumber;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            user.FullName = UserProfile.FullName;
            user.PhoneNumber = UserProfile.PhoneNumber;

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully!!!";

            return RedirectToPage();
        }
    }
}
