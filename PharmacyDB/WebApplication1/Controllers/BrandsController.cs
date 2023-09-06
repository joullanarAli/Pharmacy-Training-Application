using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Repositories;
using PharmacyInfrastructure.Shared;
using System.Net;

namespace PharmacyWeb.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BrandsController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BrandsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet(Name = "GetAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return StatusCode((int)HttpStatusCode.OK,brands);
            }catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest,ex.Message);
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string value)
        {
            try
            {
                var brands = string.IsNullOrEmpty(value) ? (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList()
                : (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList()
                .Where(e => e.Name.ToLower().Contains(value.ToLower()));
                return StatusCode((int)HttpStatusCode.OK, brands);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost(Name = "CreateBrand")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBrand(string brandName,IFormFile imageFile)
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
                    fileName = "BrandNotFound.jpg";
                }
                Brand brand = new Brand()
                {
                    Name = brandName,
                    Image = fileName,
                };
                await _unitOfWork._brandRepository.Add(brand);
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                for (int i = 0; i < brands.Count(); i++)
                {
                    brands[i].Drugs = new HashSet<Drug>();
                }
                return new ObjectResult(brands) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    
        [NonAction]
        private string UploadFile(IFormFile formFile)
        {
            string fileName = "";
            if (formFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Brands");
                fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        [HttpPut(Name ="UpdateBrand")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand([FromBody] BrandRequest brandRequestData)
        {
            try
            {
                Brand brand = await _unitOfWork._brandRepository.GetById(brandRequestData.Id);
                brand.Name = brandRequestData.Name;
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(brands) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        /* public async Task<IActionResult> UpdateBrand(int brandId,string name)
         {
             try
             {
                 Brand brand=await _unitOfWork._brandRepository.GetById(brandId);
                 brand.Name = name;
                 _unitOfWork.SaveChanges();
                 var brands=(await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                 return new ObjectResult(brands) { StatusCode = (int)HttpStatusCode.OK};
             }catch (Exception ex)
             {
                 return new ObjectResult(ex.Message) { StatusCode= (int)HttpStatusCode.BadRequest };
             }
         }*/
        [HttpPut("UpdateBrandLogo")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(int brandId, IFormFile imageFile)
        {
            try
            {
                string fileName = UploadFile(imageFile);
                Brand brand = await _unitOfWork._brandRepository.GetById(brandId);
                brand.Image = fileName;
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(brands) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete(Name ="DeleteBrand")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            try
            {
                Brand brand = await _unitOfWork._brandRepository.GetById(brandId);
                var brandDrugs = (await _unitOfWork._drugRepository.GetAll()).Where(element => element.BrandId == brandId).ToList();
                if (brandDrugs.Count > 0)
                {
                    /*string message = "You can't delete this brand because there are drugs that was made by this brand.";
                    return BadRequest(message);*/
                    for(int i = 0; i < brandDrugs.Count; i++)
                    {

                       // Drug drug = await _unitOfWork._drugRepository.GetById(brandDrugs[i].Id);
                        var drugForms= (await _unitOfWork._drugFormRepository.GetAll()).Where(e => e.DrugId==brandDrugs[i].Id);
                        var drugActiveIngredients=(await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where(e => e.DrugId == brandDrugs[i].Id);
                        if (drugForms != null)
                        {
                            foreach (var drugForm in drugForms)
                            {
                                _unitOfWork._drugFormRepository.Delete(drugForm);
                                _unitOfWork.SaveChanges();
                            }
                        }
                        if (drugActiveIngredients != null)
                        {
                            foreach (var drugActiveIngredient in drugActiveIngredients)
                            {
                                _unitOfWork._drugActiveIngredientRepository.Delete(drugActiveIngredient);
                                _unitOfWork.SaveChanges();
                            }
                        }
                        _unitOfWork._drugRepository.Delete(brandDrugs[i]);
                        _unitOfWork.SaveChanges();
                    }
                }
                _unitOfWork._brandRepository.Delete(brand);
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(brands) { StatusCode = (int)HttpStatusCode.OK };
            }catch(Exception ex)
            {
                return new ObjectResult(ex.Message) {StatusCode= (int)HttpStatusCode.BadRequest};
            }
        }
        

        /*        [HttpGet("GetBrandImage")]
        public async Task<IActionResult> GetBrandsImage(int id)
        {
            try
            {
                var brand = (await _unitOfWork._brandRepository.GetById(id));
                string filePath = "http:\\\\10.0.2.2:5191\\Images\\Brands\\" + brand.Image;
                return StatusCode((int)HttpStatusCode.OK, filePath);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }*/
    }
}
