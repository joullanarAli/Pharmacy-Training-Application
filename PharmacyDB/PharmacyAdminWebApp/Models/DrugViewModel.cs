using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyAdminWebApp.Models
{
    public class DrugViewModel
    {
        public DrugViewModel()
        {

        }
        [NotMapped]
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand is required.")]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Arabic Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Arabic Name must be between 3 and 50 characters.")]
        public string ArabicName { get; set; } = null!;
        [Required(ErrorMessage = "English Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "English Name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "English Name must start with a capital letter and must contain alphabetic characters only.")]
        public string EnglishName { get; set; } = null!;
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters.")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Side Effects is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Side Effects must be between 3 and 50 characters.")]
        public string SideEffects { get; set; } = null!;
        [Required(ErrorMessage = "Image is required")]
        public IFormFile imageFile { get; set; } = null!;

        [NotMapped]
        public List<SelectListItem> BrandNames { get; set; }
        [NotMapped]
        public List<SelectListItem> CategoryNames { get; set; }
        public List<SelectListItem> FormNames { get; set; }
        public List<SelectListItem> ActiveIngredientNames { get; set; }
        [Required(ErrorMessage = "Form is required.")]

        public List<int> SelectedFormIds { get; set; }
        [Required(ErrorMessage = "Active Ingredient is required.")]

        public List<int> SelectedActiveIngredientIds { get; set; }

        public List<float> Doses { get; set; }
        public List<float> Volumes { get; set; }


    }
}
