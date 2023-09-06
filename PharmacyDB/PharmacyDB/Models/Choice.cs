using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Choice
    {
        public Choice()
        {
         //   Answers = new HashSet<Answer>();
        }
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string ChoiceText { get; set; } = null!;
        public bool Score { get; set; }
        [JsonIgnore]
        public virtual Question Question { get; set; } = null!;
     //   public virtual ICollection<Answer> Answers { get; set; }
    }
}
