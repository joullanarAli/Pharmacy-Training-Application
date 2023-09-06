using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PharmacyDB.Models
{
    public partial class Question
    {
        public Question()
        {
            Choices = new HashSet<Choice>();
            ExamQuestions = new HashSet<ExamQuestion>();
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public int CourseId { get; set; }
        public string QuestionText { get; set; } = null!;
        public int CorrectAnswerMark { get; set; }
        public int NoAnswerMark { get; set; }
        public int WrongAnswerMark { get; set; }
        
        [NotMapped]
        public List<ExamQuestion> ExamQuestionList { get; set; } = null!;
        [JsonIgnore]
        public virtual Course Course { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Choice> Choices { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Image> Images { get; set; }
    }
}
