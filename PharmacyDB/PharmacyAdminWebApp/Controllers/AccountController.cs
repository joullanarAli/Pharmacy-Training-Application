using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PharmacyInfrastructure.View;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace PharmacyAdminWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly HttpClient _httpClient;
        private string _token;
        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager, HttpClient httpClient)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5191/Auth");
        }
       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("/Auth/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    /*var responseStream = await response.Content.ReadAsStreamAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };*/
                    /*using (var reader = new StreamReader(responseStream))
                    {
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            var jsonObject = await JObject.LoadAsync(jsonReader);

                            string token = jsonObject["message"].Value<string>();


                            TempData["AuthToken"] = token;
                        }
                    }*/
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(responseContent);

                    if (jsonObject.TryGetValue("isSuccess", out var isSuccessToken) && isSuccessToken.Value<bool>())
                    {
                        if (jsonObject.TryGetValue("message", out var tokenToken))
                        {
                            string token = tokenToken.Value<string>();

                            // Store the token in a secure way, such as in a cookie or a session
                            //  TempData["AuthToken"] = token;
                            Response.Cookies.Append("AuthToken" ,token, new CookieOptions
                            {
                                HttpOnly = true, // Prevent client-side JavaScript access
                                Secure = true,   // Set to true for HTTPS only
                                SameSite = SameSiteMode.Strict, // Apply appropriate SameSite policy
                                Expires = DateTime.UtcNow.AddHours(1) // Set cookie expiration
                            });
                            var user = new IdentityUser { UserName = model.Email };
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return Redirect("https://localhost:7097/Home/HomePage");
                        }
                    }
                }
                    else
                    {
                        return View("Error");
                    }

                }
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

