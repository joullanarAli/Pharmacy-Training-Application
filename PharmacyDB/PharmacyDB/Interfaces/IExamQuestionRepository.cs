using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDB.Interfaces
{
    public interface IExamQuestionRepository : IGenericRepository<ExamQuestion>
    {
        public List<ExamQuestion> GetAllWithQuestions();
        public List<ExamQuestion> GetAllWithExams();
    }
}
