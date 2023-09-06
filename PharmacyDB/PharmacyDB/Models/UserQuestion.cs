using System;
using System.Collections.Generic;

namespace PharmacyDB.Models
{
    public partial class UserQuestion
    {
        public UserQuestion()
        {
     //       Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public int ExamQuestionId { get; set; }
        public int UserExamId { get; set; }
        public float Mark { get; set; }

        public virtual ExamQuestion ExamQuestion { get; set; } = null!;
        public virtual UserExam UserExam { get; set; } = null!;

        //   public virtual ICollection<Answer> Answers { get; set; }
    }
}
