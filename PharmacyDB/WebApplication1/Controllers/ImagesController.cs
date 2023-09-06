using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet(Name ="GetAllImages")]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var images=(await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return Ok(images);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost(Name ="CreateImage")]
        public async  Task<IActionResult> CreateImage(int questionId,IFormFile _image)
        {
            try
            {
                string fileName;
                if (_image != null)
                {
                    fileName = UploadFile(_image);
                }
                else
                {
                    fileName = "ImageNotFound.jpg";
                }
                Image image=new Image()
                {
                    QuestionId = questionId,
                    Path = fileName,
                };
                await _unitOfWork._imageRepository.Add(image);
                _unitOfWork.SaveChanges();
                var images=(await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return Ok(images);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [NonAction]
        private string UploadFile(IFormFile formFile)
        {
            string fileName = "";
            if (formFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Quiz Images");
                fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        [HttpPut("UpdateImageQuestion")]
        public async Task<IActionResult> UpdateImageQuestion(int imageId,int questionId)
        {
            try
            {
                Image _image = await _unitOfWork._imageRepository.GetById(imageId);
                _image.QuestionId = questionId;
                _unitOfWork.SaveChanges();
                var images = (await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return Ok(images);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(Name = "UpdateImage")]
        public async Task<IActionResult> UpdateImage(int imageId, IFormFile _image)
        {
            try
            {
                string fileName = UploadFile(_image);
                Image image = await _unitOfWork._imageRepository.GetById(imageId);
                image.Path = fileName;
                _unitOfWork.SaveChanges();
                var images = (await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return Ok(images);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(Name ="DeleteImage")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                Image image = await _unitOfWork._imageRepository.GetById(imageId);
                _unitOfWork._imageRepository.Delete(image);
                _unitOfWork.SaveChanges();
                var images = (await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return Ok(images);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
