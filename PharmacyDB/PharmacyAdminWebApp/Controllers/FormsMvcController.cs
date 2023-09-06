using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class FormsMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FormsMvcController(HttpClient httpClient, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Forms");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Forms");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Forms");
                var httpClient = _httpClientFactory.CreateClient("Form");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Form>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetForms()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Forms");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Forms");
                var httpClient = _httpClientFactory.CreateClient("Forms");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Form>>(responseStream, options);
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
        public async Task<IActionResult> SearchForm(string value)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Forms");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Forms/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Form>>(responseStream, options);
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
        public async Task<IActionResult> Create(FormViewModel formVm)
        {

            if (ModelState.IsValid)
            {
                string fileName = UploadFile(formVm);

                Form form = new Form()
                {
                    Name = formVm.Name,
                    Image = fileName,
                };

                _unitOfWork.SaveChanges();
                var forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                await _unitOfWork._formRepository.Add(form);
                _unitOfWork.SaveChanges();
                forms = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");
            }
            else
            {
                return View();

            }


        }
        [NonAction]
        private string UploadFile(FormViewModel formVm)
        {

            string fileName = "";
            if (formVm.imageFile != null)
            {
                //var uri = _webHostEnvironment.WebRootPath;
                int lastSlashIndex = _webHostEnvironment.WebRootPath.LastIndexOf('\\');
                string baseUrl = lastSlashIndex >= 0 ? _webHostEnvironment.WebRootPath.Substring(0, lastSlashIndex) : _webHostEnvironment.WebRootPath;
                int beforelastSlashIndex = baseUrl.LastIndexOf('\\');
                string BaseUrl = beforelastSlashIndex >= 0 ? baseUrl.Substring(0, beforelastSlashIndex) : baseUrl;
                // string baseUrl = uri.GetLeftPart(UriPartial.Path);
                string uploadDir = Path.Combine(BaseUrl, "WebApplication1", "wwwroot", "Images", "Forms");
                fileName = Guid.NewGuid().ToString() + "-" + formVm.imageFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formVm.imageFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        public async Task<IActionResult> GetModal(int id)
        {
            Form form = await _unitOfWork._formRepository.GetById(id);
            return PartialView("Modal", form);
        }

        public async Task<IActionResult> GetModalImage(int id)
        {
            Form form = await _unitOfWork._formRepository.GetById(id);
            FormViewModel formVm = new FormViewModel();
            formVm.Id = id;
            formVm.Name = form.Name;
            return PartialView("ModalImage", formVm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Form _form)
        {

            Form form = new Form
            {
                Id = _form.Id,
                Name = _form.Name,
                Image = _form.Image,
            };
            FormRequest requestData = new FormRequest
            {
                Id = form.Id,
                Name = form.Name
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Forms");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Forms", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Form>>(responseStream, options);
                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Error");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditImage(FormViewModel _formVm)
        {
            if (ModelState.IsValid)
            {
                string fileName = UploadFile(_formVm);
                Form form = await _unitOfWork._formRepository.GetById(_formVm.Id);
                form.Image = fileName;
                _unitOfWork.SaveChanges();
                var brands = (await _unitOfWork._formRepository.GetAll()).Reverse().ToList();
                return RedirectToAction("Index");

            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView("Delete", await _unitOfWork._formRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFormMvc(int id)
        {
            Form form = (await _unitOfWork._formRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Forms");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Forms?formId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Form>>(responseStream, options);
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
            Form form = await _unitOfWork._formRepository.GetById(id);
            return View(form);
        }
        public async Task<IActionResult> GetFormDrugs(int formId)
        {
            var drugForms = (await _unitOfWork._drugFormRepository.GetAll()).Where(e => e.FormId == formId).ToList();
            List<Drug> drugs = new List<Drug>();
            foreach (var drugForm in drugForms)
            {
                Drug drug = await _unitOfWork._drugRepository.GetById(drugForm.DrugId);
                drugs.Add(drug);
            }
            return Json(new
            {
                success = true,
                message = "All Data is back",
                data = drugs
            });

        }
    }
}
