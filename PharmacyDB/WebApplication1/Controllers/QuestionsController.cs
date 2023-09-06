using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyWeb.Models;
using System.Net;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : BaseController
    {
        public QuestionsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet(Name ="GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var questions= (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList();
                return Ok(questions);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetQuestionExams")]
        public async Task<IActionResult> GetQuestionExams(int questionId)
        {
            try
            {
                var questionExams = (await _unitOfWork._examQuestionRepository.GetAll()).Where((element) => element.QuestionId == questionId).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, questionExams);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("GetQuestionsByExam")]
        public async Task<IActionResult> GetQuestionsByExam(int examId)
        {
            try
            {
                List<Question> questions = new List<Question>();

                var examQuestions = (await _unitOfWork._examQuestionRepository.GetAll()).Where((element) => element.ExamId ==examId).Reverse().ToList();
                for (int i = 0; i < examQuestions.Count; i++)
                {
                    Question question = (await _unitOfWork._questionRepository.GetById(examQuestions[i].QuestionId));
                    questions.Add(question);
                }
                return StatusCode((int)HttpStatusCode.OK, questions);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string value)
        {
            try
            {
                var questions = string.IsNullOrEmpty(value) ? (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList()
                .Where(e => e.QuestionText.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, questions);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost(Name ="CreateQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateQuestion([FromBody] Question question)
        {
            try
            {
               await _unitOfWork._questionRepository.Add(question);
                _unitOfWork.SaveChanges();
                var questions= (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList();
                int questionId = questions.First().Id;
                if (question.ExamQuestionList!=null)
                {
                    for (int i = 0; i < question.ExamQuestionList.Count; i++)
                    {
                        ExamQuestion examQuestion = new ExamQuestion()
                        {
                            QuestionId = questionId,
                            ExamId = question.ExamQuestionList[i].ExamId
                        };
                        await _unitOfWork._examQuestionRepository.Add(examQuestion);
                        _unitOfWork.SaveChanges();
                    }
                }
                return Ok(questions);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name ="UpdateQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionViewModel questionVm)
        {
            try
            {
                Question _question = await _unitOfWork._questionRepository.GetById(questionVm.Id);
                _question.NoAnswerMark = questionVm.NoAnswerMark;
                _question.QuestionText = questionVm.QuestionText;
                _question.CorrectAnswerMark = questionVm.CorrectAnswerMark;
                _question.WrongAnswerMark = questionVm.WrongAnswerMark;
                _question.CourseId = questionVm.CourseId;
                _unitOfWork.SaveChanges();
                var questions = (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList();
                return Ok(questions);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(Name ="DeleteQuestion")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            try
            {
                Question question = await _unitOfWork._questionRepository.GetById(questionId);
                var questionchoices= (await _unitOfWork._choiceRepository.GetAll()).Where(element => element.QuestionId==questionId).ToList();
                var questionExams=(await _unitOfWork._examQuestionRepository.GetAll()).Where(element => element.QuestionId == questionId).ToList();
                List<Image> questionImages=(await _unitOfWork._imageRepository.GetAll()).Where(element=>element.QuestionId==questionId).ToList();
                if (questionImages.Count>0)
                {
                    for (int i = 0; i < questionImages.Count; i++)
                    {
                        _unitOfWork._imageRepository.Delete(questionImages[i]);
                    }
                }
                for(int i = 0; i < questionExams.Count; i++)
                {
                    var exams = await _unitOfWork._examRepository.GetAll();
                    var exam = exams.First(element => element.Id == questionExams[i].ExamId);
                    var examQuestions = (await _unitOfWork._examQuestionRepository.GetAll()).Where(element => element.ExamId == exam.Id).ToList();
                    _unitOfWork._examQuestionRepository.Delete(questionExams[i]);
                    _unitOfWork.SaveChanges();
                    if (examQuestions.Count == 1)
                    {
                        _unitOfWork._examRepository.Delete(exam);
                        _unitOfWork.SaveChanges();
                    }
                   // _unitOfWork._examQuestionRepository.Delete(questionExams[i]);
                }
                for (int i = 0; i < questionchoices.Count; i++)
                {
                    _unitOfWork._choiceRepository.Delete(questionchoices[i]);
                    _unitOfWork.SaveChanges();
                }
                _unitOfWork._questionRepository.Delete(question);
                _unitOfWork.SaveChanges();
                var questions = (await _unitOfWork._questionRepository.GetAll()).Reverse().ToList();
                return Ok(questions);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
