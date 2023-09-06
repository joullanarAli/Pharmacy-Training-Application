using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyDB.Models;

namespace PharmacyWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly PharmacyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SetupController> _logger;
        public SetupController(PharmacyDbContext context,  UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<SetupController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            var roleExist= await _roleManager.RoleExistsAsync(name);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(name));
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation($"The Role {name} has been added successfully");
                    return Ok(new
                    {
                        result = $"The Role {name} has been added successfully"
                    });
                }
                else
                {
                    _logger.LogInformation($"The Role {name} has not been added");
                    return Ok(new
                    {
                        result = $"The Role {name} has not been added"
                    });
                }
            }
            else
            {
                return BadRequest(new { error = "Role is already exist" });
            }
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                _logger.LogInformation($"The User with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The Role {roleName} does not Exist");
                return BadRequest(new
                {
                    error= $"The Role {roleName} does not Exist"
                });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = "Success, user has been added to the role"
                });
                
            }
            else {
                _logger.LogInformation($"The User was not able to be added");
                return BadRequest(new
                {
                    error = $"The User was not able to be added"
                });
            }
        }
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The User with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email,string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogInformation($"The User with the {email} does not exist");
                return BadRequest(new
                {
                    error = "User does not exist"
                });
            }
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                _logger.LogInformation($"The Role {roleName} does not Exist");
                return BadRequest(new
                {
                    error = $"The Role {roleName} does not Exist"
                });
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    result = $"User {email} has been removed from role {roleName}"
                }) ;
            }
            return BadRequest(new
            {
                error= $"Unable to remove User {email} from Role {roleName}"
            });
        }
    }

}
