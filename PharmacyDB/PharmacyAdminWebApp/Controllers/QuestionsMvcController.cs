using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyAdminWebApp.Models;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using PharmacyInfrastructure.Requests;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class QuestionsMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QuestionsMvcController(HttpClient httpClient, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Questions");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Questions");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetQuestions()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Questions");
                var httpClient = _httpClientFactory.CreateClient("Questions");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);
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
        public async Task<IActionResult> SearchQuestion(string value)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Questions/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Question _question)
        {
            _question.CourseId = 1;
             _question.ExamQuestionList=new List<ExamQuestion>();
            
            
            Question question = new Question()
            {
                QuestionText = _question.QuestionText,
                WrongAnswerMark = _question.WrongAnswerMark,
                CorrectAnswerMark = _question.CorrectAnswerMark,
                NoAnswerMark = _question.NoAnswerMark,
                CourseId = 1,
            };

            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.PostAsync("/Questions", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);
                    return RedirectToAction("Index");
                }
                else
                {

                    return View();

                }
            }
            
        }
        public async Task<IActionResult> GetModal(int id)
        {
            Question question = await _unitOfWork._questionRepository.GetById(id);
            return PartialView("Modal", question);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Question _question)
        {

            Question question = new Question
            {
                Id = _question.Id,
                QuestionText = _question.QuestionText,
                WrongAnswerMark = _question.WrongAnswerMark,
                CorrectAnswerMark = _question.CorrectAnswerMark,
                NoAnswerMark = _question.NoAnswerMark,
                CourseId = _question.CourseId,
            };
            var content = new StringContent(JsonConvert.SerializeObject(question), Encoding.UTF8, "application/json");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Questions", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);

                    return RedirectToAction("Index");

                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            return PartialView("Delete", await _unitOfWork._questionRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteQuestionMvc(int id)
        {
            Question question = (await _unitOfWork._questionRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Questions?questionId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Question>>(responseStream, options);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetDetails(int id)
        {
            return PartialView("Details", await _unitOfWork._questionRepository.GetById(id));
        }
        public async Task<IActionResult> Choices(int id)
        {
            var choices = (await _unitOfWork._choiceRepository.GetAll()).Where(element => element.QuestionId == id).ToList();
            return View("Questionchoices",choices);
        }
        public async Task<IActionResult> GetQuestionChoices(int questionId)
        {
            var choices = (await _unitOfWork._choiceRepository.GetAll()).Where(element => element.QuestionId == questionId);
            return Json(new
            {
                success = true,
                message = "All Data is back",
                data = choices
            });
        }
        public async Task<IActionResult> AddChoice(int questionId)
        {
            var choices=(await _unitOfWork._choiceRepository.GetAll()).Where(e => e.QuestionId==questionId).ToList();
            return View("AddChoice");
        }
        public async Task<IActionResult> AddChoiceToQuestion(PharmacyDB.Models.Choice _choice)
        {
            Choice choice = new Choice()
            {
                ChoiceText = _choice.ChoiceText,
                QuestionId = _choice.QuestionId,
            };

            var content = new StringContent(JsonConvert.SerializeObject(choice), Encoding.UTF8, "application/json");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.PostAsync("/Choices", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Choice>>(responseStream, options);
                    return View("QuestionChoices",data);
                }
                else
                {

                    return View();

                }
            }
        }
        public async Task<IActionResult> GetChoiceModal(int id)
        {
            PharmacyDB.Models.Choice choice = await _unitOfWork._choiceRepository.GetById(id);
            return PartialView("ChoiceModal", choice);
        }
        [HttpPost]
        public async Task<IActionResult> EditChoice(PharmacyDB.Models.Choice _choice)
        {

            PharmacyDB.Models.Choice choice = new PharmacyDB.Models.Choice
            {
                Id = _choice.Id,
                ChoiceText = _choice.ChoiceText,
                QuestionId = _choice.QuestionId,
                Score = _choice.Score,
            };
            var content = new StringContent(JsonConvert.SerializeObject(choice), Encoding.UTF8, "application/json");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Choices", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<PharmacyDB.Models.Choice>>(responseStream, options);

                    return View("QuestionChoices", data);

                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> DeleteChoiceModal(int id)
        {
            return PartialView("DeleteChoice", await _unitOfWork._choiceRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteChoiceMvc(int id)
        {
            PharmacyDB.Models.Choice choice = (await _unitOfWork._choiceRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Questions");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Choices?choiceId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<PharmacyDB.Models.Choice>>(responseStream, options);
                    return View("QuestionChoices", data); 
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> Images(int id)
        {
            var images = (await _unitOfWork._imageRepository.GetAll()).Where(element => element.QuestionId == id).ToList();
            return View("Images", images);
        }
        public async Task<IActionResult> GetQuestionImages(int questionId)
        {
            var images = (await _unitOfWork._imageRepository.GetAll()).Where(element => element.QuestionId == questionId);
            return Json(new
            {
                success = true,
                message = "All Data is back",
                data = images
            });
        }
        public async Task<IActionResult> AddImage(int questionId)
        {
            var images = (await _unitOfWork._imageRepository.GetAll()).Where(e => e.QuestionId == questionId).ToList();
            return View("AddImage");
        }
        public async Task<IActionResult> AddImageToQuestion(ImageViewModel _imageVm)
        {
            string fileName = UploadFile(_imageVm);
            Image image = new Image()
            {
                QuestionId = _imageVm.QuestionId,
                Path = fileName,
            };

            _unitOfWork.SaveChanges();
                var images = (await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                await _unitOfWork._imageRepository.Add(image);
                _unitOfWork.SaveChanges();
                images = (await _unitOfWork._imageRepository.GetAll()).Reverse().ToList();
                return View("Images",images);
            
            
                 
                
            
        }
        [NonAction]
        private string UploadFile(ImageViewModel imageVm)
        {

            string fileName = "";
            if (imageVm.Path != null)
            {
                //var uri = _webHostEnvironment.WebRootPath;
                int lastSlashIndex = _webHostEnvironment.WebRootPath.LastIndexOf('\\');
                string baseUrl = lastSlashIndex >= 0 ? _webHostEnvironment.WebRootPath.Substring(0, lastSlashIndex) : _webHostEnvironment.WebRootPath;
                int beforelastSlashIndex = baseUrl.LastIndexOf('\\');
                string BaseUrl = beforelastSlashIndex >= 0 ? baseUrl.Substring(0, beforelastSlashIndex) : baseUrl;
                // string baseUrl = uri.GetLeftPart(UriPartial.Path);
                string uploadDir = Path.Combine(BaseUrl, "WebApplication1", "wwwroot", "Images", "Quiz Images");
                fileName = Guid.NewGuid().ToString() + "-" + imageVm.Path.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageVm.Path.CopyTo(fileStream);
                }
            }
            return fileName;
        }
        public async Task<IActionResult> DeleteImageModal(int id)
        {
            return PartialView("DeleteImage", await _unitOfWork._imageRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteImageMvc(int id)
        {
            Image image = (await _unitOfWork._imageRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Images");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Images?imageId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Image>>(responseStream, options);
                    return View("Images", data);
                }
                else
                {
                    return View("Error");
                }
            }
        }

    }
}
