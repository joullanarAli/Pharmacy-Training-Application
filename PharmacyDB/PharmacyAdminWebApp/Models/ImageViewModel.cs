using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyAdminWebApp.Models
{
    public partial class ImageViewModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public IFormFile Path { get; set; } = null!;
        [JsonIgnore]
        public virtual Question Question { get; set; } = null!;
    }
}
