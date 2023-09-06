using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Models;
using System.Security.Claims;

namespace PharmacyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        public readonly PharmacyDbContext _context;
        public readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly ILogger<ClaimsController> _logger;
        public ClaimsController(PharmacyDbContext context,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<ClaimsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllClaims(string email)
        {
            var user= await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User Does not exist"
                });
            }
            var userClaims= await _userManager.GetClaimsAsync(user);
            return Ok(userClaims);
        }
        [HttpPost]
        [Route("AddClaimToUser")]
        public async Task<IActionResult> AddClaimToUser(string email, string claimName, string claimValue)
        {
            var user= await _userManager.FindByIdAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The user with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User Does not exist"
                });
            }
            var userClaim = new Claim(claimName, claimValue);
            var result = await  _userManager.AddClaimAsync(user, userClaim);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"user {user.Email} has a claim {claimName} added to them"
                });
            }
            else
            {
                return BadRequest(new
                {
                    error = $"unable to add claim {claimName} to the user {user.Email}"
                });
            }
        }
    }
}
