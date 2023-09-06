using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Shared;
using System.Net;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FormsController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FormsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet(Name = "GetAllForms")]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, forms);
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
                var forms = string.IsNullOrEmpty(value) ? (await _unitOfWork._formRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._formRepository.GetAll()).Reverse().ToList()
                .Where(e => e.Name.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, forms);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost(Name = "CreateForm")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateForm(string name,IFormFile formFile)
        {
            try
            {
                string fileName= UploadFile(formFile);
                Form form = new Form()
                {
                    Name = name,
                    Image = fileName,
                };
                await _unitOfWork._formRepository.Add(form);
                _unitOfWork.SaveChanges();
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(forms) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [NonAction]
        [Authorize(Roles = "Admin")]
        private string UploadFile(IFormFile formFile)
        {
            string fileName = "";
            string filePath = "";
            if (formFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Forms");
                fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        [HttpPut(Name = "UpdateForm")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Updateform([FromBody] FormRequest formRequestData)
        {
            try
            {
                Form form = await _unitOfWork._formRepository.GetById(formRequestData.Id);
                form.Name = formRequestData.Name;
                _unitOfWork.SaveChanges();
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(forms) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpPut("UpdateFormImage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateformImage(int formId, IFormFile imageFile)
        {
            try
            {
                string fileName = UploadFile(imageFile);
                Form form = await _unitOfWork._formRepository.GetById(formId);
                form.Image = fileName;
                _unitOfWork.SaveChanges();
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(forms) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        [HttpDelete(Name = "DeleteForm")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteForm(int formId)
        {
            try
            {
                Form form = await _unitOfWork._formRepository.GetById(formId);
                var formDrugs= (await _unitOfWork._drugFormRepository.GetAll()).Where(element => element.FormId==formId).ToList();
                if (formDrugs.Count > 0)
                {
                    string message = "You can't delete this Form because there are drugs that have this form.";
                    return BadRequest(message);
                }
                _unitOfWork._formRepository.Delete(form);
                _unitOfWork.SaveChanges();
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(forms) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
