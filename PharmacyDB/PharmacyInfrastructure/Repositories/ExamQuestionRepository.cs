using PharmacyDB.Models;
using PharmacyDB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PharmacyInfrastructure.Repositories
{
    public class ExamQuestionRepository : GenericRepository<ExamQuestion> , IExamQuestionRepository
    {
        public ExamQuestionRepository(PharmacyDbContext context) : base(context)
        {

        }
        public List<ExamQuestion> GetAllWithQuestions()
        {
            return _context.ExamQuestions.Include(e => e.Question)
                .ToList();
        }
        public List<ExamQuestion> GetAllWithExams()
        {
            return _context.ExamQuestions.Include(e => e.Exam)
                .ToList();
        }
    }
}
