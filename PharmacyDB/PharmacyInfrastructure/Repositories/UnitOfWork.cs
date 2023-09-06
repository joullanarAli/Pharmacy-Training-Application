using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyInfrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PharmacyDbContext _context;
        public IDrugRepository _drugRepository { get; }
        public IDrugFormRepository _drugFormRepository { get; }
        public IDrugActiveIngredientRepository _drugActiveIngredientRepository { get; }
        public IFormRepository _formRepository { get; }
        public IActiveIngredientRepository _activeIngredientRepository { get; }
        public IBrandRepository _brandRepository { get; }
        public ICategoryRepository _categoryRepository { get; }
        public ICourseRepository _courseRepository { get; }
        public IExamQuestionRepository _examQuestionRepository { get; }
        public IQuestionRepository _questionRepository { get; }
        public IExamRepository _examRepository { get; }
       // public IUserExamRepository _userExamRepository { get; }
       // public IUserQuestionRepository _userQuestionRepository { get; }
        public IImageRepository _imageRepository { get; }
        public IChoiceRepository _choiceRepository { get; }
    //    public IAnswerRepository _answerRepository { get; }
        public UnitOfWork(PharmacyDbContext context)
        {
            _context = context;
            _drugRepository = new DrugRepository(_context);
            _drugFormRepository = new DrugFormRepository(_context);
            _drugActiveIngredientRepository = new DrugActiveIngredientRepository(_context);
            _formRepository = new FormRepository(_context);
            _activeIngredientRepository = new ActiveIngredientRepository(_context);
            _brandRepository = new BrandRepository(_context);
            _categoryRepository = new CategoryRepository(_context);
            _courseRepository = new CourseRepository(_context);
            _examQuestionRepository = new ExamQuestionRepository(_context);
            _examRepository = new ExamRepository(_context);
            _questionRepository = new QuestionRepository(_context);
          //  _userExamRepository = new UserExamRepository(_context);
        //    _userQuestionRepository = new UserQuestionRepository(_context);
            _imageRepository = new ImageRepository(_context);
            _choiceRepository = new ChoiceRepository(_context);
            //_answerRepository = new AnswerRepository(_context);
        }
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
