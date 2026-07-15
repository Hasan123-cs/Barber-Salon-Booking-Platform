using System.ComponentModel.DataAnnotations;

namespace BarberSalon.Models.BindingModel
{
    public class CreateEmployeeBindingModel
    {
            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MinLength(6)]
            public string Password { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Phone { get; set; }

            [Required]
            public string Specialization { get; set; }

            [Range(0, 50)]
            public int ExperienceYears { get; set; }

            public bool IsActive { get; set; } = true;
    }
}
