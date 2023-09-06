using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Drug
    {
        public Drug()
        {
            DrugActiveIngredients = new HashSet<DrugActiveIngredient>();
            DrugForms = new HashSet<DrugForm>();
        }
        
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
        public string Image { get; set; } = null!;
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters.")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Side Effects is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Side Effects must be between 3 and 50 characters.")]
        public string SideEffects { get; set; } = null!;
        [NotMapped]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile imageFile { get; set; } 

        [NotMapped]
        public List<DrugForm> DrugFormsList { get; set; }=null!;
        [NotMapped]
        public List<DrugActiveIngredient> DrugActiveIngredientsList { get; set; } = null!;


        public virtual Brand Brand { get; set; } =null!;


        public virtual Category Category { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<DrugActiveIngredient> DrugActiveIngredients { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<DrugForm> DrugForms { get; set; } = null!;
    }
}
