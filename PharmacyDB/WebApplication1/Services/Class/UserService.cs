using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PharmacyInfrastructure;
using PharmacyInfrastructure.View;
using PharmacyWeb.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PharmacyWeb.Services.Class
{
    public class UserService : IUserService
    {
        private IConfiguration _configuration;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserService(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }
        public async Task<UserManagerResponse> RegisterUser(RegisterViewModel model)
        {
            if(model == null)
                throw new NullReferenceException("Register Model is null");
            if(model.Password != model.ConfirmedPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirmed Password doesn't match password",
                    IsSuccess = false,
                };
            }
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };
            var result = await _userManager.CreateAsync(identityUser,model.Password);
            if (result.Succeeded)
            {
                var resultRoleAddition = await _userManager.AddToRoleAsync(identityUser, "Admin"); 
                return new UserManagerResponse
                {
                    Message = "User created successfully",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse
            {
                Message = "User did not created",
                IsSuccess = false,
                Errors = result.Errors.Select(x => x.Description)
            };
        }
        public async Task<UserManagerResponse> LoginUser(LoginViewModel model)
        {
            var user= await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Wrong Email Address",
                    IsSuccess=false,
                };
            }
            var result= await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Password",
                    IsSuccess = false,
                };
            }
            /*var claims = new[]
            {
                 new Claim("Email",model.Email),
                 new Claim(ClaimTypes.NameIdentifier,user.Id)
             };*/
            var claims = await GetAllValidClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                ); ;
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
           // var jwtToken = GenerateJwtTokenAsync(user);
             return new UserManagerResponse
             {
                 Message =tokenString,
                 IsSuccess = true,
                 ExpireDate = token.ValidTo

             };
        }
        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection(key: "JwtConfig:Secret").Value);

            var claims = await GetAllValidClaims(user);

            //token descriptor 
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }
        private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
        {
            var _options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim("Id",user.Id),
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()),
            };

            //getting the claims that we have assigned to the user 
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            //get the user role and add it to the claims 

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;

        }
    }
}
