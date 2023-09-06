using PharmacyDB.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PharmacyAdminWebApp.Models
{
    public class BrandViewModel
    {
            public BrandViewModel()
            {
                Drugs = new HashSet<Drug>();
            }
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public IFormFile imageFile { get; set; } = null!;
            [JsonIgnore]
            public virtual ICollection<Drug> Drugs { get; set; }
        
    }
}
