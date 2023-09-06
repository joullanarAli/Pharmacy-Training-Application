using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyAdminWebApp.Models;
using PharmacyDB.Interfaces;
using PharmacyDB.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class ExamsMvcController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExamsMvcController(HttpClient httpClient, IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment) : base(unitOfWork)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Exams");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Exams");
                var httpClient = _httpClientFactory.CreateClient("Exams");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);
                    return View(data);
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetExams()
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Exams");
                var httpClient = _httpClientFactory.CreateClient("Exams");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);
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
        public async Task<IActionResult> SearchExam(string value)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Exams/Search?value=" + value);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);
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
        public async Task<IActionResult> Create(Exam _exam)
        {
            if (ModelState.IsValid)
            {
                Exam exam = new Exam()
                {
                    Name = _exam.Name,
                };

                var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
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
                    httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                    HttpResponseMessage response = await httpClientWithCookies.PostAsync("/Exams", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = await response.Content.ReadAsStreamAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);
                        return RedirectToAction("Index");
                    }
                    else
                    {

                        return View();

                    }
                }
            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> GetModal(int id)
        {
            Exam exam = await _unitOfWork._examRepository.GetById(id);
            return PartialView("Modal", exam);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Exam _exam)
        {

            Exam exam = new Exam
            {
                Id = _exam.Id,
                Name = _exam.Name,
            };
            var content = new StringContent(JsonConvert.SerializeObject(exam), Encoding.UTF8, "application/json");
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.PutAsync("/Exams", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);

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
            return PartialView("Delete", await _unitOfWork._examRepository.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteExamMvc(int id)
        {
            Exam exam = (await _unitOfWork._examRepository.GetById(id));
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.DeleteAsync($"/Exams?examId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Exam>>(responseStream, options);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }
        }
        public async Task<IActionResult> GetExamQuestions(int examId)
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
                httpClientWithCookies.BaseAddress = new Uri("http://localhost:5191/Exams");
                HttpResponseMessage response = await httpClientWithCookies.GetAsync("/Exams/GetExamQuestions?examId=" + examId);
                // var httpClient = _httpClientFactory.CreateClient("Drugs");
                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<ExamQuestion>>(responseStream, options);
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
        public async Task<IActionResult> Questions(int id)
        {
            Exam exam = await _unitOfWork._examRepository.GetById(id);
            return View(exam);
        }
        public async Task<IActionResult> GetQuestionModal(int id)
        {
            ExamQuestion examQuestion = await _unitOfWork._examQuestionRepository.GetById(id);
            return PartialView("QuestionModal", examQuestion);
        }
        [HttpPost]
        public async Task<IActionResult> EditQuestion(ExamQuestion examQuestion)
        {
            _unitOfWork._examQuestionRepository.Update(examQuestion);
            _unitOfWork.SaveChanges();
            Exam exam = await _unitOfWork._examRepository.GetById(examQuestion.ExamId);
            return RedirectToAction("Questions", exam);
        }
        
        public async Task<IActionResult> DeleteQuestionModal(int examQuestionId)
        {

            return PartialView("DeleteQuestion", await _unitOfWork._examQuestionRepository.GetById(examQuestionId));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            ExamQuestion examQuestion = await _unitOfWork._examQuestionRepository.GetById(id);
            int examId = examQuestion.ExamId;
            _unitOfWork._examQuestionRepository.Delete(examQuestion);
            _unitOfWork.SaveChanges();
            var examQuestions = (await _unitOfWork._examQuestionRepository.GetAll()).Where(o => o.ExamId == examId).ToList();
            Exam exam = await _unitOfWork._examRepository.GetById(examQuestion.ExamId);
            return RedirectToAction("Questions", exam);
        }
        public async Task<IActionResult> AddQuestionToExam(int examId)
        {
            var availableQuestions = (await _unitOfWork._questionRepository.GetAll()).ToList();
            var viewModel = new AddQuestionToExamViewModel
            {
                ExamId = examId,
                AvailableQuestions = availableQuestions
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestionToExam(AddQuestionToExamViewModel viewModel)
        {

            ExamQuestion examQuestion = new ExamQuestion
            {
                ExamId = viewModel.ExamId,
                QuestionId = viewModel.SelectedQuestionId
            };
            await _unitOfWork._examQuestionRepository.Add(examQuestion);
            _unitOfWork.SaveChanges();
            Exam exam = await _unitOfWork._examRepository.GetById(viewModel.ExamId);
            return RedirectToAction("Questions", "ExamsMvc", exam);

            viewModel.AvailableQuestions = (await _unitOfWork._questionRepository.GetAll()).ToList();
        }
        public async Task<IActionResult> Choices(int id)
        {
            var choices = (await _unitOfWork._choiceRepository.GetAll()).Where(element => element.QuestionId == id).ToList();
            return View("Choices", choices);
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
    }
}
