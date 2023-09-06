using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Requests
{
    public class QuestionRequest
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string QuestionText { get; set; } = null!;
        public int CorrectAnswerMark { get; set; }
        public int NoAnswerMark { get; set; }
        public int WrongAnswerMark { get; set; }
        public List<ChoiceRequest> Choices { get; set; } = null!;
    }
}
