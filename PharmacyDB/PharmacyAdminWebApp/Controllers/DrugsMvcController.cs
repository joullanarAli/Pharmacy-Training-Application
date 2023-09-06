using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PharmacyAdminWebApp.Models;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Shared;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class DrugsMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DrugsMvcController(HttpClient httpClient, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Drugs");
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Drugs");
                var httpClient = _httpClientFactory.CreateClient("Drugs");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Drug>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetDrugs()
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Drugs");
                var httpClient = _httpClientFactory.CreateClient("Drugs");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Drug>>(responseStream, options);
                    return Json(new
                    {
                        success = true,
                        message = "All Data is back",
                        data = data
                    });
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> SearchDrug(string value)
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Drugs/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Drug>>(responseStream, options);
                    return Json(new
                    {
                        success = true,
                        message = "All Data is back",
                        data = data
                    });
                }
                else
                {
                    // Handle error cases
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> Create()
        {
            List<SelectListItem> brandList = new List<SelectListItem>();
            List<SelectListItem> categoryList = new List<SelectListItem>();
            List<SelectListItem> formList = new List<SelectListItem>();
            List<SelectListItem> activeIngredientList = new List<SelectListItem>();
            DrugViewModel drugViewModel = new DrugViewModel();
            var brands = (await _unitOfWork._brandRepository.GetAll()).ToList();
            var categories = (await _unitOfWork._categoryRepository.GetAll()).ToList();
            var forms = (await _unitOfWork._formRepository.GetAll()).ToList();
            var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).ToList();
            foreach (var brand in brands)
            {
                brandList.Add(new SelectListItem
                {
                    Text = brand.Name,
                    Value = brand.Id.ToString()
                });
            }
            drugViewModel.BrandNames = brandList;
            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }
            drugViewModel.CategoryNames = categoryList;

            foreach (var form in forms)
            {
                formList.Add(new SelectListItem
                {
                    Text = form.Name,
                    Value = form.Id.ToString()
                });
            }
            drugViewModel.FormNames = formList;
            foreach (var activeIngredient in activeIngredients)
            {
                activeIngredientList.Add(new SelectListItem
                {
                    Text = activeIngredient.Name,
                    Value = activeIngredient.Id.ToString()
                });
            }
            drugViewModel.ActiveIngredientNames = activeIngredientList;
            return View(drugViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(DrugViewModel drugVm)
        {
            /*if (ModelState.IsValid)
            {*/
            var fileName = UploadFile(drugVm);
            if (drugVm.SelectedFormIds != null)
            {
                var _drug = new Drug();
                _drug.BrandId = drugVm.BrandId;
                _drug.CategoryId = drugVm.CategoryId;
                _drug.EnglishName = drugVm.EnglishName;
                _drug.ArabicName = drugVm.ArabicName;
                _drug.Description = drugVm.Description;
                _drug.SideEffects = drugVm.SideEffects;
                _drug.Image = fileName;
                _unitOfWork._drugRepository.Add(_drug);
                _unitOfWork.SaveChanges();
                for (int i = 0; i < drugVm.SelectedFormIds.Count; i++)
                {
                    var drugForm = new DrugForm();
                    drugForm.DrugId = _drug.Id;
                    drugForm.FormId = drugVm.SelectedFormIds[i];
                    drugForm.Dose = drugVm.Doses[i];
                    drugForm.Volume = drugVm.Volumes[i];
                    _unitOfWork._drugFormRepository.Add(drugForm);
                    _unitOfWork.SaveChanges();

                }
                for (int i = 0; i < drugVm.SelectedActiveIngredientIds.Count; i++)
                {
                    var drugActiveIngredient = new DrugActiveIngredient();
                    drugActiveIngredient.DrugId = _drug.Id;
                    drugActiveIngredient.ActiveIngredientId = drugVm.SelectedActiveIngredientIds[i];
                    _unitOfWork._drugActiveIngredientRepository.Add(drugActiveIngredient);
                    _unitOfWork.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> brandList = new List<SelectListItem>();
                List<SelectListItem> categoryList = new List<SelectListItem>();
                List<SelectListItem> formList = new List<SelectListItem>();
                List<SelectListItem> activeIngredientList = new List<SelectListItem>();
                DrugViewModel drugViewModel = new DrugViewModel();
                var brands = (await _unitOfWork._brandRepository.GetAll()).ToList();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).ToList();
                var forms = (await _unitOfWork._formRepository.GetAll()).ToList();
                var activeIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).ToList();
                foreach (var brand in brands)
                {
                    brandList.Add(new SelectListItem
                    {
                        Text = brand.Name,
                        Value = brand.Id.ToString()
                    });
                }
                drugViewModel.BrandNames = brandList;
                foreach (var category in categories)
                {
                    categoryList.Add(new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
                drugViewModel.CategoryNames = categoryList;
                foreach (var form in forms)
                {
                    categoryList.Add(new SelectListItem
                    {
                        Text = form.Name,
                        Value = form.Id.ToString()
                    });
                }
                drugViewModel.FormNames = formList;
                foreach (var activeIngredient in activeIngredients)
                {
                    activeIngredientList.Add(new SelectListItem
                    {
                        Text = activeIngredient.Name,
                        Value = activeIngredient.Id.ToString()
                    });
                }
                drugViewModel.ActiveIngredientNames = activeIngredientList;
                return View(drugViewModel);
            }


            /*}
            else
            {
                List<SelectListItem> brandList = new List<SelectListItem>();
                List<SelectListItem> categoryList = new List<SelectListItem>();
                List<SelectListItem> formList = new List<SelectListItem>();
                DrugViewModel drugViewModel = new DrugViewModel();
                var brands = (await _unitOfWork._brandRepository.GetAll()).ToList();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).ToList();
                var forms = (await _unitOfWork._formRepository.GetAll()).ToList();

                foreach (var brand in brands)
                {
                    brandList.Add(new SelectListItem
                    {
                        Text = brand.Name,
                        Value = brand.Id.ToString()
                    });
                }
                drugViewModel.BrandNames = brandList;
                foreach (var category in categories)
                {
                    categoryList.Add(new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }
                drugViewModel.CategoryNames = categoryList;
                foreach (var form in forms)
                {
                    categoryList.Add(new SelectListItem
                    {
                        Text = form.Name,
                        Value = form.Id.ToString()
                    });
                }
                drugViewModel.FormNames = categoryList;
                return View(drugViewModel);*/
        }


        /* string fileName = UploadFile(drugVm);

         Drug drug = new Drug()
         {
             EnglishName = drugVm.EnglishName,
             ArabicName = drugVm.ArabicName,
             Description = drugVm.Description,
             SideEffects = drugVm.SideEffects,
             BrandId = drugVm.BrandId,
             CategoryId = drugVm.CategoryId,
             Image = fileName,

         };
     drug.DrugForms = drugVm.SelectedFormIds.Select(formId => new DrugForm
     {
         DrugId = drug.Id,
         FormId = formId,
     }).ToList();
         var brands = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
         await _unitOfWork._drugRepository.Add(drug);
         _unitOfWork.SaveChanges();
         brands = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
         TempData["Message"] = "Data Created Successfully";
         return RedirectToAction("Index");
     */

        //   }


        [NonAction]
        private string UploadFile(DrugViewModel brandVm)
        {

            string fileName = "";
            if (brandVm.imageFile != null)
            {
                int lastSlashIndex = _webHostEnvironment.WebRootPath.LastIndexOf('\\');
                string baseUrl = lastSlashIndex >= 0 ? _webHostEnvironment.WebRootPath.Substring(0, lastSlashIndex) : _webHostEnvironment.WebRootPath;
                int beforelastSlashIndex = baseUrl.LastIndexOf('\\');
                string BaseUrl = beforelastSlashIndex >= 0 ? baseUrl.Substring(0, beforelastSlashIndex) : baseUrl;
                string uploadDir = Path.Combine(BaseUrl, "WebApplication1", "wwwroot", "Images", "Drugs");
                fileName = Guid.NewGuid().ToString() + "-" + brandVm.imageFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    brandVm.imageFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        public async Task<IActionResult> GetModalAsync(int id)
        {
            //  Brand brand = (await _unitOfWork._brandRepository.GetAll()).First(element => element.Id == id);

            return PartialView("Modal", await _unitOfWork._drugRepository.GetById(id));
        }
        [HttpPost]
        public IActionResult Edit(Drug drug)
        {
            /*DrugRequest requestData = new DrugRequest
            {
                Id = drug.Id,
                EnglishName = drug.EnglishName,
                ArabicName = drug.ArabicName,
                Description = drug.Description,
                SideEffects = drug.SideEffects,
                BrandId = drug.BrandId,
                CategoryId = drug.CategoryId,
                Image=drug.Image,
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync("/Drugs/UpdateDrug",content);
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Drug>>(responseStream, options);
                TempData["Message"] = "Data Updated Successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error");
            }*/
            _unitOfWork._drugRepository.Update(drug);
            _unitOfWork.SaveChanges();
            var data = _unitOfWork._drugRepository.GetAll();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> GetModalImage(int id)
        {
            Drug drug = (await _unitOfWork._drugRepository.GetById(id));
            DrugViewModel drugVm = new DrugViewModel {
                Id = id,
                EnglishName = drug.EnglishName,
                ArabicName = drug.ArabicName,
                Description = drug.Description,
                SideEffects = drug.SideEffects,
            };
        
            return PartialView("ModalImage", drugVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(DrugViewModel _drugVm)
        {
            /*if (ModelState.IsValid)
            {*/
                string fileName = UploadFile(_drugVm);
                Drug drug = await _unitOfWork._drugRepository.GetById(_drugVm.Id);
                drug.Image = fileName;
                _unitOfWork.SaveChanges();
                var drugs = (await _unitOfWork._drugRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");

           /* }
            else
            {
                return View("Error");
            }*/
        }
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView("Delete", await _unitOfWork._drugRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDrugMvc(int id)
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Drugs?drugId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Drug>>(responseStream, options);
                    // var data = await response.Content.ReadAsAsync<Brand>();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> DeleteForm(int id)
        {
            var drugForms = (await _unitOfWork._drugFormRepository.GetAll()).ToList().Where(o => o.DrugId == id).ToList();
            Drug drug = await _unitOfWork._drugRepository.GetById(id);
            _unitOfWork._drugRepository.Delete(drug);
            _unitOfWork.SaveChanges();
            TempData["Message"] = "Data Deleted Successfully";
            return RedirectToAction("Index");
        }*/
        public async Task<IActionResult> Details(int id)
        {
            Drug drug = await _unitOfWork._drugRepository.GetById(id);
            return View(drug);
        }
        public async Task<IActionResult> GetDrugForms(int drugId)
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Drugs/GetDrugForms?drugId=" + drugId);
                var httpClient = _httpClientFactory.CreateClient("Drugs");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<DrugForm>>(responseStream, options);
                    return Json(new
                    {
                        success = true,
                        message = "All Data is back",
                        data = data
                    });
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> Forms(int id)
        {
            Drug drug = await _unitOfWork._drugRepository.GetById(id);
            return View(drug);
        }
        public async Task<IActionResult> GetFormModal(int id)
        {
            DrugForm drugForm = await _unitOfWork._drugFormRepository.GetById(id);
            return PartialView("FormModal", drugForm);
        }
        [HttpPost]
        public async Task<IActionResult> EditForm(DrugForm drugForm)
        {
            _unitOfWork._drugFormRepository.Update(drugForm);
            _unitOfWork.SaveChanges();
            Drug drug = await _unitOfWork._drugRepository.GetById(drugForm.DrugId);
            return RedirectToAction("Forms", drug);
        }
        public async Task<IActionResult> DeleteFormModal(int drugFormId)
        {

            return PartialView("DeleteForm", await _unitOfWork._drugFormRepository.GetById(drugFormId));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForm(int id)
        {
            DrugForm drugForm = await _unitOfWork._drugFormRepository.GetById(id);
            int drugId = drugForm.DrugId;
            _unitOfWork._drugFormRepository.Delete(drugForm);
            _unitOfWork.SaveChanges();
            var drugForms = (await _unitOfWork._drugFormRepository.GetAll()).Where(o => o.DrugId == drugId).ToList();
            Drug drug = await _unitOfWork._drugRepository.GetById(drugForm.DrugId);
            return  RedirectToAction("Forms", drug);
        }
        public async Task<IActionResult> GetDetails(int id)
        {
            return PartialView("DrugDetails", await _unitOfWork._drugRepository.GetById(id));
        }
        public async Task<IActionResult> ActiveIngredients(int id)
        {
            Drug drug = await _unitOfWork._drugRepository.GetById(id);
            return View(drug);
        }
        public async Task<IActionResult> GetDrugActiveIngredients(int drugId)
        {
            var cookies = Request.Cookies;
            var cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookieContainer.Add(_httpClient.BaseAddress, new Cookie(cookie.Key, cookie.Value));
            }
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer
            };
            using (var httpClientWithCookies = new HttpClient(httpClientHandler))
            {
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Drugs");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Drugs/GetDrugActiveIngredients?drugId=" + drugId);
                var httpClient = _httpClientFactory.CreateClient("Drugs");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<DrugActiveIngredient>>(responseStream, options);
                    return Json(new
                    {
                        success = true,
                        message = "All Data is back",
                        data = data
                    });
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> DeleteActiveIngredientModal(int drugActiveIngredientId)
        {

            return PartialView("DeleteActiveIngredient", await _unitOfWork._drugActiveIngredientRepository.GetById(drugActiveIngredientId));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteActiveIngredient(int id)
        {
            DrugActiveIngredient drugActiveIngredient = await _unitOfWork._drugActiveIngredientRepository.GetById(id);
            int drugId = drugActiveIngredient.DrugId;
            _unitOfWork._drugActiveIngredientRepository.Delete(drugActiveIngredient);
            _unitOfWork.SaveChanges();
            var drugActiveIngredients = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where(o => o.DrugId == drugId).ToList();
            Drug drug = await _unitOfWork._drugRepository.GetById(drugActiveIngredient.DrugId);
            return RedirectToAction("ActiveIngredients", drug);
        }
        public async Task<IActionResult> AddFormToDrug(int drugId)
        {
            var availableForms = (await _unitOfWork._formRepository.GetAll()).ToList(); 
            var viewModel = new AddFormToDrugViewModel
            {
                DrugId = drugId,
                AvailableForms = availableForms
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddFormToDrug(AddFormToDrugViewModel viewModel)
        {

            DrugForm drugForm = new DrugForm
            {
                DrugId = viewModel.DrugId,
                FormId = viewModel.SelectedFormId,
                Dose = viewModel.Dose,
                Volume = viewModel.Volume,
            };
            await _unitOfWork._drugFormRepository.Add(drugForm);
            _unitOfWork.SaveChanges();
            Drug drug=await _unitOfWork._drugRepository.GetById(viewModel.DrugId);
            return RedirectToAction("Forms", "DrugsMvc",drug); 

            viewModel.AvailableForms = (await _unitOfWork._formRepository.GetAll()).ToList();
        }
        public async Task<IActionResult> AddActiveIngredientToDrug(int drugId)
        {
            var availableActiveIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).ToList();
            var drugActiveIngredients = (await _unitOfWork._drugActiveIngredientRepository.GetAll()).Where(element => element.DrugId == drugId).ToList();
            for(int i = 0; i < availableActiveIngredients.Count; i++)
            {
                for(int j = 0; j < drugActiveIngredients.Count; j++)
                {
                    if (availableActiveIngredients[i].Id == drugActiveIngredients[j].ActiveIngredientId)
                    {
                        availableActiveIngredients.Remove(availableActiveIngredients[i]);
                    }
                }
            }
            var viewModel = new AddActiveIngredientToDrugViewModel
            {
                DrugId = drugId,
                AvailableActiveIngredients = availableActiveIngredients
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddActiveIngredientToDrug(AddActiveIngredientToDrugViewModel viewModel)
        {
            
            DrugActiveIngredient drugActiveIngredient = new DrugActiveIngredient
            {
                DrugId = viewModel.DrugId,
                ActiveIngredientId = viewModel.SelectedActiveIngredientId,
            };
            await _unitOfWork._drugActiveIngredientRepository.Add(drugActiveIngredient);
            _unitOfWork.SaveChanges();
            Drug drug = await _unitOfWork._drugRepository.GetById(viewModel.DrugId);
            return RedirectToAction("ActiveIngredients", "DrugsMvc", drug);

            viewModel.AvailableActiveIngredients = (await _unitOfWork._activeIngredientRepository.GetAll()).ToList();
        }
    }
}
