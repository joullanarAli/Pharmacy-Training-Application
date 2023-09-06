using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyDB.Interfaces
{
    public interface IUnitOfWork
    {
        int SaveChanges();
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
      //  public IUserExamRepository _userExamRepository { get; }
       // public IUserQuestionRepository _userQuestionRepository { get; }
        public IImageRepository _imageRepository { get; }
        public IChoiceRepository _choiceRepository { get; }
      //  public IAnswerRepository _answerRepository { get; }
    }
}
