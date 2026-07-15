using System.ComponentModel.DataAnnotations;

namespace BarberSalon.Models.BindingModel
{
    public class CreateCategoryBindingModel
    {
        [Required(ErrorMessage ="Name Field Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description Field Is Required")]
        [StringLength(500)]

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
