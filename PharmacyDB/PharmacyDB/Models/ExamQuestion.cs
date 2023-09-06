using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class ExamQuestion
    {
        public ExamQuestion()
        {
        }

        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public virtual Exam Exam { get; set; } = null!;

        public virtual Question Question { get; set; } = null!;
    }
}
