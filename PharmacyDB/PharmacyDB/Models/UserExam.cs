using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PharmacyDB.Models
{
    public partial class UserExam
    {
        public UserExam()
        {
            UserQuestions = new HashSet<UserQuestion>();
        }

        public int Id { get; set; }
        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
        public virtual ICollection<UserQuestion> UserQuestions { get; set; }
    }
}
