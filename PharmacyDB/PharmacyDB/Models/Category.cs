using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Category
    {
        public Category()
        {
            Drugs = new HashSet<Drug>();
        }
        

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Name must start with a capital letter and must contain alphabetic characters only.")]
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        [NotMapped]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile imageFile { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Drug> Drugs { get; set; }
    }
}
