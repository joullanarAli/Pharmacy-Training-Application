using PharmacyDB.Models;

namespace PharmacyAdminWebApp.Models
{
    public class AddQuestionToExamViewModel
    {
        public int ExamId { get; set; }
        public int SelectedQuestionId { get; set; }
        public List<Question> AvailableQuestions { get; set; }
    }
}
