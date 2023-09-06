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
    public class CategoriesController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet (Name="GetCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, categories);
            }catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string value)
        {
            try
            {
                var categories = string.IsNullOrEmpty(value) ? (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList()
                .Where(e => e.Name.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, categories);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost(Name ="AddCategory")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateCategoty(string categoryName, IFormFile imageFile)
        {
            try
            {
                string fileName;
                if (imageFile != null)
                {
                    fileName = UploadFile(imageFile);
                }
                else
                {
                    fileName = "CategoryNotFound.jpg";
                }
                Category category = new Category()
                {
                    Name = categoryName,
                    Image = fileName,
                };
                await _unitOfWork._categoryRepository.Add(category);
                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                for (int i = 0; i < categories.Count(); i++)
                {
                    categories[i].Drugs = new HashSet<Drug>();
                }
                return new ObjectResult(categories) { StatusCode = (int)HttpStatusCode.OK };
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
            string fileName="";
            if (formFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Categories");
                fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
                //Console.WriteLine(filePath);
            }
            return fileName;
        }

        [HttpPut(Name ="UpdateCategory")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest categoeyRequestData)
        {
            try
            {
                Category category=await _unitOfWork._categoryRepository.GetById(categoeyRequestData.Id);
                category.Name=categoeyRequestData.Name;
                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, categories);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpPut("UpdateCategoryImage")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateCategoryImage(int categoryId, IFormFile imageFile)
        {
            try
            {
                string fileName = UploadFile(imageFile);
                Category category = await _unitOfWork._categoryRepository.GetById(categoryId);
                category.Image = fileName;
                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, categories);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpDelete(Name ="DeleteCategory")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                Category category = await _unitOfWork._categoryRepository.GetById(categoryId);
                var categoryDrugs = (await _unitOfWork._drugRepository.GetAll()).Where(element => element.CategoryId == categoryId).ToList();
                if (categoryDrugs.Count > 0)
                {
                    string message = "You can't delete this category because there are drugs that was made by this category.";
                    return BadRequest(message);
                }
                _unitOfWork._categoryRepository.Delete(category);
                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK, categories);
            }catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
