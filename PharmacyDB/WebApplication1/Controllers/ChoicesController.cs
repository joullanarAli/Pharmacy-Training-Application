using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
   // [Authorize]
    public class ChoicesController : BaseController
    {
        public ChoicesController (IUnitOfWork unitOfWork) :base(unitOfWork)
        {

        }
        [HttpGet(Name ="GetAllChoices")]
        public async Task<IActionResult> GetAllChoices()
        {
            try
            {
                var choices = (await _unitOfWork._choiceRepository.GetAll()).Reverse().ToList();
                return Ok(choices);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(Name ="CreateChoice")]
        public async Task<IActionResult> CreateChoice(Choice choice)
        {
            try
            {
                await _unitOfWork._choiceRepository.Add(choice);
                _unitOfWork.SaveChanges();
                var choices = (await _unitOfWork._choiceRepository.GetAll()).Reverse().ToList();
                return Ok(choices);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name ="UpdateChoice")]
        public async Task<IActionResult> UpdateChoice(Choice choice)
        {
            try
            {
                Choice _choice = await _unitOfWork._choiceRepository.GetById(choice.Id);
                _choice.Score=choice.Score;
                _choice.QuestionId=choice.QuestionId;
                _choice.ChoiceText  =choice.ChoiceText;
                _unitOfWork.SaveChanges();
                var choices = (await _unitOfWork._choiceRepository.GetAll()).Reverse().ToList();
                return Ok(choices);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpDelete(Name ="DeleteChoice")]
        public async Task<IActionResult> DeleteChoice(int choiceId)
        {
            try
            {
                Choice choice = await _unitOfWork._choiceRepository.GetById(choiceId);
                _unitOfWork._choiceRepository.Delete(choice);
                _unitOfWork.SaveChanges();
                var choices = (await _unitOfWork._choiceRepository.GetAll()).Reverse().ToList();
                return Ok(choices);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
