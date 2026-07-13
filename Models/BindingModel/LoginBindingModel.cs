using System.ComponentModel.DataAnnotations;

namespace BarberSalon.Models.BindingModel
{
    public class LoginBindingModel
    {
        [EmailAddress]
        [Required]  
        public string email { get; set; }
        [MinLength(6,ErrorMessage ="Password At Least contain 6 character")]
        public string password { get; set; }
    }
}
