using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyAdminWebApp.Models;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Repositories;
using PharmacyInfrastructure.Shared;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class CategoriesMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoriesMvcController(HttpClient httpClient, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment,IUnitOfWork unitOfWork):base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Categories");
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Categories");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Categories");
                var httpClient = _httpClientFactory.CreateClient("Categories");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Category>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetCategories()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Categories");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Categories");
                var httpClient = _httpClientFactory.CreateClient("Categories");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Category>>(responseStream, options);
                    // return View(data);
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
        public async Task<IActionResult> SearchCategory(string value)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Categories");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Categories/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Category>>(responseStream, options);
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryVm)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadFile(categoryVm);

                Category category = new Category()
                {
                    Name = categoryVm.Name,
                    Image = fileName,
                };

                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                await _unitOfWork._categoryRepository.Add(category);
                _unitOfWork.SaveChanges();
                categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");
            }
            else
            {

                return View();

            }
        }
        [NonAction]
        private string UploadFile(CategoryViewModel categoryVm)
        {

            string fileName = "";
            if (categoryVm.imageFile != null)
            {
                //var uri = _webHostEnvironment.WebRootPath;
                int lastSlashIndex = _webHostEnvironment.WebRootPath.LastIndexOf('\\');
                string baseUrl = lastSlashIndex >= 0 ? _webHostEnvironment.WebRootPath.Substring(0, lastSlashIndex) : _webHostEnvironment.WebRootPath;
                int beforelastSlashIndex = baseUrl.LastIndexOf('\\');
                string BaseUrl = beforelastSlashIndex >= 0 ? baseUrl.Substring(0, beforelastSlashIndex) : baseUrl;
                // string baseUrl = uri.GetLeftPart(UriPartial.Path);
                string uploadDir = Path.Combine(BaseUrl, "WebApplication1", "wwwroot", "Images", "Categories");
                fileName = Guid.NewGuid().ToString() + "-" + categoryVm.imageFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    categoryVm.imageFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        public async Task<IActionResult> GetModal(int id)
        {
            Category category = await _unitOfWork._categoryRepository.GetById(id);
            return PartialView("Modal", category);
        }

        public async Task<IActionResult> GetModalImage(int id)
        {
            Category category = await _unitOfWork._categoryRepository.GetById(id);
            CategoryViewModel categoryVm = new CategoryViewModel();
            categoryVm.Id = id;
            categoryVm.Name = category.Name;
            return PartialView("ModalImage", categoryVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category _category)
        {

            Category category = new Category
            {
                Id = _category.Id,
                Name = _category.Name,
                Image = _category.Image,
            };
            CategoryRequest requestData = new CategoryRequest
            {
                Id = category.Id,
                Name = category.Name
            };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Categories");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Categories", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Category>>(responseStream, options);

                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Error");
                }
            }
        }
        [HttpPost]
       public async Task<IActionResult> EditImage(CategoryViewModel _categoryVm)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadFile(_categoryVm);
                Category category = await _unitOfWork._categoryRepository.GetById(_categoryVm.Id);
                category.Image = fileName;
                _unitOfWork.SaveChanges();
                var categories = (await _unitOfWork._categoryRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView("Delete", await _unitOfWork._categoryRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategoryMvc(int id)
        {
            Category category = (await _unitOfWork._categoryRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Categories");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Categories?categoryId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Category>>(responseStream, options);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> Drugs(int id)
        {
            Category category = await _unitOfWork._categoryRepository.GetById(id);
            return View(category);
        }
        public async Task<IActionResult> GetCategoryDrugs(int categoryId)
        {
            var drugs = (await _unitOfWork._drugRepository.GetAll()).Where(e => e.CategoryId == categoryId).ToList();

            // var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<DrugForm>>(responseStream, options);
            return Json(new
            {
                success = true,
                message = "All Data is back",
                data = drugs
            });

        }
        public async Task<IActionResult> GetDrugModal(int id)
        {
            Drug drug = await _unitOfWork._drugRepository.GetById(id);
            return PartialView("DrugModal", drug);
        }
    }
}
