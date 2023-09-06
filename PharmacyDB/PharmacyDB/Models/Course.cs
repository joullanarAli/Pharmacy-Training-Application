using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Course
    {
        public Course()
        {
            Questions = new HashSet<Question>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
