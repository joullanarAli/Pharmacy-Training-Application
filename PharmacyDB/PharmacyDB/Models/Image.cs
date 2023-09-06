using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Image
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Path { get; set; } = null!;
        [JsonIgnore]
        public virtual Question Question { get; set; } = null!;
    }
}
