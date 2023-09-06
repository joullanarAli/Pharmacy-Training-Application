using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyAdminWebApp.Models;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Shared;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class BrandsMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandsMvcController(HttpClient httpClient,IUnitOfWork unitOfWork ,IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Brands");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Brands");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Brands");

                var httpClient = _httpClientFactory.CreateClient("Brands");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Brand>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetBrands()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Brands");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Brands");
                var httpClient = _httpClientFactory.CreateClient("Brands");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Brand>>(responseStream, options);
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
        public async Task<IActionResult> SearchBrand(string value)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Brands");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Brands/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Brand>>(responseStream, options);
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
        public async Task<IActionResult> Create(BrandViewModel brandVm)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadFile(brandVm);

                Brand brand = new Brand()
                {
                    Name = brandVm.Name,
                    Image = fileName,
                };

                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                await _unitOfWork._brandRepository.Add(brand);
                _unitOfWork.SaveChanges();
                brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");
            }
            else
            {

                return View();

            }


        }
        [NonAction]
        private string UploadFile(BrandViewModel brandVm)
        {

            string fileName = "";
            if (brandVm.imageFile != null)
            {
                //var uri = _webHostEnvironment.WebRootPath;
                int lastSlashIndex = _webHostEnvironment.WebRootPath.LastIndexOf('\\');
                string baseUrl = lastSlashIndex >= 0 ? _webHostEnvironment.WebRootPath.Substring(0, lastSlashIndex) : _webHostEnvironment.WebRootPath;
                int beforelastSlashIndex = baseUrl.LastIndexOf('\\');
                string BaseUrl = beforelastSlashIndex >= 0 ? baseUrl.Substring(0, beforelastSlashIndex) : baseUrl;
                // string baseUrl = uri.GetLeftPart(UriPartial.Path);
                string uploadDir = Path.Combine(BaseUrl, "WebApplication1", "wwwroot", "Images", "Brands");
                fileName = Guid.NewGuid().ToString() + "-" + brandVm.imageFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    brandVm.imageFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        public async Task<IActionResult> GetModal(int id)
        {
            Brand brand = (await _unitOfWork._brandRepository.GetAll()).First(element => element.Id == id);
            return PartialView("Modal", brand);
        }


        public async Task<IActionResult> GetModalImage(int id)
        {
            Brand brand = (await _unitOfWork._brandRepository.GetAll()).First(element => element.Id == id);
            BrandViewModel brandVm = new BrandViewModel();
            brandVm.Id = id;
            brandVm.Name = brand.Name;
            return PartialView("ModalImage", brandVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand _brand)
        {
            Brand brand = new Brand
            {
                Id = _brand.Id,
                Name = _brand.Name,
                Image = _brand.Image,
            };
            BrandRequest requestData = new BrandRequest
            {
                Id = brand.Id,
                Name = brand.Name
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Brands");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Brands", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Brand>>(responseStream, options);

                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Error");
                }
            }
            
            
        }
        [HttpPost]
        public async Task<IActionResult> EditImage(BrandViewModel _brandVm)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadFile(_brandVm);
                Brand brand = await _unitOfWork._brandRepository.GetById(_brandVm.Id);
                brand.Image = fileName;
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._brandRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView("Delete", await _unitOfWork._brandRepository.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrandMvc(int id)
        {
            Brand brand = (await _unitOfWork._brandRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Brands");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Brands?brandId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Brand>>(responseStream, options);
                    // var data = await response.Content.ReadAsAsync<Brand>();
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
            Brand brand = await _unitOfWork._brandRepository.GetById(id);
            return View(brand);
        }
        public async Task<IActionResult> GetBrandDrugs(int brandId)
        {
            var drugs = (await _unitOfWork._drugRepository.GetAll()).Where(e => e.BrandId == brandId).ToList();

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
   

