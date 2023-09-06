using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyInfrastructure.View;
using PharmacyWeb.Services.Interface;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger)
        {
            _userService = userService;
            _roleManager = roleManager;
            _logger = logger;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result=await _userService.RegisterUser(model);
               return Ok(result);
            }
            return BadRequest("Some properties are not valid");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result=await _userService.LoginUser(model);
                return Ok(result);
            }
            return BadRequest("Some properties are not valid");
        }
    }
}
