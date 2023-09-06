using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System.Net;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ExamsController : BaseController
    {
        public ExamsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet(Name ="GetAllExams")]
        public async Task<IActionResult> GetAllExams()
        {
            try
            {
                var exams=(await _unitOfWork._examRepository.GetAll()).Reverse().ToList();
                
                return Ok(exams);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetExamQuestions")]
        public async Task<IActionResult> GetExamQuestions(int examId)
        {
            try
            {
                //var drugForms = (await _unitOfWork._drugFormRepository.GetAll()).Where((element)=>element.DrugId==drugId).Reverse().ToList();
                var examQuestions = _unitOfWork._examQuestionRepository.GetAllWithQuestions().Where(element => element.ExamId == examId).Reverse().ToList();
                foreach (var examQuestion in examQuestions)
                {
                    examQuestion.Question = await _unitOfWork._questionRepository.GetById(examQuestion.QuestionId);
                    examQuestion.Exam = await _unitOfWork._examRepository.GetById(examQuestion.ExamId);
                    _unitOfWork.SaveChanges();
                }
                return StatusCode((int)HttpStatusCode.OK, examQuestions);
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
                var exams = string.IsNullOrEmpty(value) ? (await _unitOfWork._examRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._examRepository.GetAll()).Reverse().ToList()
                .Where(e => e.Name.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, exams);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost(Name = "CreateExam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateExam(Exam exam)
        {
            try
            {
                await _unitOfWork._examRepository.Add(exam);
                _unitOfWork.SaveChanges();
                var exams = (await _unitOfWork._examRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(exams) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpPut(Name ="UpdateExam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateExam(Exam exam)
        {
            try
            {
                Exam _exam = await _unitOfWork._examRepository.GetById(exam.Id);
                _exam.Name = exam.Name;
                _unitOfWork.SaveChanges();
                var exams = (await _unitOfWork._examRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(exams) { StatusCode = (int)HttpStatusCode.OK};
            }catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpDelete(Name ="DeleteExam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            try
            {
                Exam exam = await _unitOfWork._examRepository.GetById(examId);
                var examQuestions= (await _unitOfWork._examQuestionRepository.GetAll()).Where(element => element.ExamId==examId).ToList();
                for(int i = 0; i < examQuestions.Count; i++)
                {
                    _unitOfWork._examQuestionRepository.Delete(examQuestions[i]);
                    _unitOfWork.SaveChanges();
                }
                _unitOfWork._examRepository.Delete(exam);
                _unitOfWork.SaveChanges();
                var exams = (await _unitOfWork._examRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(exams) { StatusCode = (int)HttpStatusCode.OK };
            }catch (Exception ex)
            {
                return new ObjectResult(ex) { StatusCode= (int)HttpStatusCode.BadRequest };
            }
        }
       
    }
}
