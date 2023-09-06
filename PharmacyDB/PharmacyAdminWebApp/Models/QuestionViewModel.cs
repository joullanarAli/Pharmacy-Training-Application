using PharmacyDB.Models;

namespace PharmacyAdminWebApp.Models
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string QuestionText { get; set; } = null!;
        public int CorrectAnswerMark { get; set; }
        public int NoAnswerMark { get; set; }
        public int WrongAnswerMark { get; set; }
        List<Choice> Choices { get; set; }
    }
}
