using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System.Net;

namespace PharmacyWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DrugsController : BaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DrugsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet(Name ="GetAllDrugs")]
        public async Task<IActionResult> GetAllDrugs()
        {
            try
            {
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                foreach (var drug in drugs)
                {
                    drug.Brand= await _unitOfWork._brandRepository.GetById(drug.BrandId);
                    drug.Category = await _unitOfWork._categoryRepository.GetById(drug.CategoryId);
                    _unitOfWork.SaveChanges();
                }
                
                return StatusCode((int)HttpStatusCode.OK, drugs);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("GetDrugForms")]
        public async Task<IActionResult> GetDrugForms(int drugId)
        {
            try
            {
                //var drugForms = (await _unitOfWork._drugFormRepository.GetAll()).Where((element)=>element.DrugId==drugId).Reverse().ToList();
                var drugForms =  _unitOfWork._drugFormRepository.GetAllWithForms().Where(element => element.DrugId == drugId).Reverse().ToList();
                foreach (var drugForm in drugForms)
                {
                    drugForm.Form=await _unitOfWork._formRepository.GetById(drugForm.FormId);
                    drugForm.Drug = await _unitOfWork._drugRepository.GetById(drugForm.DrugId);
                    _unitOfWork.SaveChanges();
                }
                return StatusCode((int)HttpStatusCode.OK, drugForms);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("GetDrugsByForm")]
        public async Task<IActionResult> GetDrugsByForm(int formId)
        {
            try
            {
                List<Drug> drugs = new List<Drug>();

                var formDrugs = (await _unitOfWork._drugFormRepository.GetAll()).Where((element) => element.FormId == formId).Reverse().ToList();
                for (int i = 0; i < formDrugs.Count; i++)
                {
                    Drug drug = (await _unitOfWork._drugRepository.GetById(formDrugs[i].DrugId));
                    drugs.Add(drug);
                }
                return StatusCode((int)HttpStatusCode.OK, drugs);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("GetDrugActiveIngredients")]
        public async Task<IActionResult> GetDrugActiveIngredients(int drugId)
        {
            try
            {
                var drugActiveIngredients = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where((element) => element.DrugId == drugId).Reverse().ToList();
                foreach (var drugActiveIngredient in drugActiveIngredients)
                {
                    drugActiveIngredient.ActiveIngredient = await _unitOfWork._activeIngredientRepository.GetById(drugActiveIngredient.ActiveIngredientId);
                    drugActiveIngredient.Drug = await _unitOfWork._drugRepository.GetById(drugActiveIngredient.DrugId);
                    _unitOfWork.SaveChanges();
                }
                return StatusCode((int)HttpStatusCode.OK, drugActiveIngredients);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpGet("GetDrugsByActiveIngredient")]
        public async Task<IActionResult> GetDrugsByActiveIngredient(int activeIngredientId)
        {
            try
            {
                List<Drug> drugs = new List<Drug>();

                var activeIngredientDrugs = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where((element) => element.ActiveIngredientId == activeIngredientId).Reverse().ToList();
                for (int i = 0; i < activeIngredientDrugs.Count; i++)
                {
                    Drug drug = (await _unitOfWork._drugRepository.GetById(activeIngredientDrugs[i].DrugId));
                    drugs.Add(drug);
                }
                return StatusCode((int)HttpStatusCode.OK, drugs);
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
                var drugs = string.IsNullOrEmpty(value) ? (await _unitOfWork._drugRepository.GetAll()).ToList()
                : (await _unitOfWork._drugRepository.GetAll()).ToList()
                .Where(e => e.EnglishName.ToLower().Contains(value.ToLower()) ||
                e.ArabicName.ToLower().Contains(value.ToLower()));
                foreach (var drug in drugs)
                {
                    drug.Brand = await _unitOfWork._brandRepository.GetById(drug.BrandId);
                    drug.Category = await _unitOfWork._categoryRepository.GetById(drug.CategoryId);
                    _unitOfWork.SaveChanges();
                }
                return StatusCode((int)HttpStatusCode.OK, drugs);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
        }
        [HttpPost(Name = "CreateDrug")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDrug([FromBody] Drug _drug)       
        {
            try
            { 
                _drug.Image = " ";
                await _unitOfWork._drugRepository.Add(_drug);
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                int drugId=drugs.First().Id;
                for(int i = 0; i < _drug.DrugFormsList.Count; i++) {
                    DrugForm drugForm = new DrugForm()
                    {
                        DrugId = drugId,
                        FormId = _drug.DrugFormsList[i].FormId,
                        Volume = _drug.DrugFormsList[i].Volume,
                        Dose = _drug.DrugFormsList[i].Dose
                    };
                    await _unitOfWork._drugFormRepository.Add(drugForm);
                    _unitOfWork.SaveChanges();
                }
                for (int i = 0; i < _drug.DrugActiveIngredientsList.Count; i++)
                {
                    DrugActiveIngredient drugActiveIngredient = new DrugActiveIngredient()
                    {
                        DrugId = drugId,
                        ActiveIngredientId= _drug.DrugActiveIngredientsList[i].ActiveIngredientId,
                    };
                    await _unitOfWork._drugActiveIngredientRepository.Add(drugActiveIngredient);
                    _unitOfWork.SaveChanges();
                }
                drugs=(await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return Ok(drugs);
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [HttpPost("AddDrugImage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDrugImage(IFormFile imageFile,int drugId)
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
                    fileName = "DrugNotFound.jpg";
                }
                var _drug = await _unitOfWork._drugRepository.GetById(drugId);
                _drug.Image = fileName;
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return Ok(drugs);
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
            if (formFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Drugs");
                fileName = Guid.NewGuid().ToString() + "-" + formFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut(Name = "UpdateDrug")]
        public async Task<IActionResult> UpdateDrug(int drugId, Drug _drug)
        {
            try
            {
                Drug drug = await _unitOfWork._drugRepository.GetById(drugId);
                drug.BrandId = _drug.BrandId;
                drug.CategoryId = _drug.CategoryId;
                drug.ArabicName = _drug.ArabicName;
                drug.EnglishName = _drug.EnglishName;
                drug.SideEffects = _drug.SideEffects;
                drug.Description = _drug.Description;
            /*    drug.DrugFormsList.Clear();
                drug.DrugActiveIngredientsList.Clear();*/
                var drugFormsList=(await _unitOfWork._drugFormRepository.GetAll()).Where((element)=>element.DrugId==drug.Id).Reverse().ToList();
                for(int i = 0; i < drugFormsList.Count; i++)
                {
                    _unitOfWork._drugFormRepository.Delete(drugFormsList[i]);
                    _unitOfWork.SaveChanges();
                }
                var drugActiveIngredientsList = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where((element) => element.DrugId == drug.Id).Reverse().ToList();
                for (int i = 0; i < drugActiveIngredientsList.Count; i++)
                {
                    _unitOfWork._drugActiveIngredientRepository.Delete(drugActiveIngredientsList[i]);
                    _unitOfWork.SaveChanges();
                }
                for (int i = 0; i < _drug.DrugFormsList.Count; i++)
                {
                    DrugForm drugForm = new DrugForm()
                    {
                        DrugId = drugId,
                        FormId = _drug.DrugFormsList[i].FormId,
                        Volume = _drug.DrugFormsList[i].Volume,
                        Dose = _drug.DrugFormsList[i].Dose
                    };
                    await _unitOfWork._drugFormRepository.Add(drugForm);
                    _unitOfWork.SaveChanges();
                }
                for (int i = 0; i < _drug.DrugActiveIngredientsList.Count; i++)
                {
                    DrugActiveIngredient drugActiveIngredient = new DrugActiveIngredient()
                    {
                        DrugId = drugId,
                        ActiveIngredientId = _drug.DrugActiveIngredientsList[i].ActiveIngredientId,
                    };
                    await _unitOfWork._drugActiveIngredientRepository.Add(drugActiveIngredient);
                    _unitOfWork.SaveChanges();
                }
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(drugs) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDrugImage")]
        public async Task<IActionResult> UpdatDrugImage(int drugId,IFormFile imageFile)
        {
            try
            {
                string fileName=UploadFile(imageFile);
                Drug drug = await _unitOfWork._drugRepository.GetById(drugId);
                drug.Image = fileName;
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(drugs) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete(Name = "DeleteDrug")]
        public async Task<IActionResult> DeleteDrug(int drugId)
        {
            try
            {
                Drug drug = await _unitOfWork._drugRepository.GetById(drugId);
                List<DrugForm> drugsForm = (await _unitOfWork._drugFormRepository.GetAll()).Where((element)=> element.DrugId==drugId).ToList();
                for(int i = 0; i < drugsForm.Count; i++)
                {
                     _unitOfWork._drugFormRepository.Delete(drugsForm[i]);
                    _unitOfWork.SaveChanges();
                }
                List<DrugActiveIngredient> drugsActiveIngredients= (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where((element) => element.DrugId == drugId).ToList();
                for (int i = 0; i < drugsActiveIngredients.Count; i++)
                {
                    _unitOfWork._drugActiveIngredientRepository.Delete(drugsActiveIngredients[i]);
                    _unitOfWork.SaveChanges();
                }
                _unitOfWork._drugRepository.Delete(drug);
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return new ObjectResult(drugs) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message) { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

    }
}
