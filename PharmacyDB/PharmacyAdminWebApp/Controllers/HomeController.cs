using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyAdminWebApp.Models;
using PharmacyAdminWebApp.Services;
using PharmacyDB.Interfaces;
using System.Diagnostics;


namespace PharmacyAdminWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<IdentityUser> _signInManager;

        //   private readonly ISingletonService _singletonService;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
  //          _singletonService = singletonService;
        }

        public IActionResult Index()
        {
            _signInManager.SignOutAsync();
            return View();
        }
        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}