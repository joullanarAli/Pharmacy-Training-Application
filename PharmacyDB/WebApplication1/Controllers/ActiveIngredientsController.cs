using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActiveIngredientsController : BaseController
    {
        public ActiveIngredientsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet(Name = "GetAllActiveIngredients")]
        public async Task<IActionResult> GetAllActiveIngredients()
        {
            try
            {
                var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, activeIngredients);
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
                var activeIngredients = string.IsNullOrEmpty(value) ? (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList()
                .Where(e => e.Name.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, activeIngredients);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost(Name = "CreateActiveIngredient")]
        public async Task<IActionResult> CreateActiveIngredient(ActiveIngredient activeIngredient)
        {
            try
            {
                await _unitOfWork._activeIngredientRepository.Add(activeIngredient);
                _unitOfWork.SaveChanges();
                var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList();
               
                return new ObjectResult(activeIngredients) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpPut(Name = "UpdateactiveIngredient")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateActiveIngredient(ActiveIngredient _activeIngredient)
        {
            try
            {
                ActiveIngredient activeIngredient = await _unitOfWork._activeIngredientRepository.GetById(_activeIngredient.Id);
                activeIngredient.Name = _activeIngredient.Name;
                _unitOfWork.SaveChanges();
                var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(activeIngredients) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [Authorize(Roles = "Admin")]

        [HttpDelete(Name = "DeleteActiveIngredient")]
        public async Task<IActionResult> DeleteActiveIngredient(int activeIngredientId)
        {
            try
            {
                ActiveIngredient activeIngredient = await _unitOfWork._activeIngredientRepository.GetById(activeIngredientId);
                var activeIngredientDrugs = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where(element => element.ActiveIngredientId == activeIngredientId).ToList();
                if (activeIngredientDrugs.Count > 0)
                {
                    string message = "You can't delete this active ingredient because there are drugs that have it.";
                    return BadRequest(message);
                }
                _unitOfWork._activeIngredientRepository.Delete(activeIngredient);
                _unitOfWork.SaveChanges();
                var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(activeIngredients) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
