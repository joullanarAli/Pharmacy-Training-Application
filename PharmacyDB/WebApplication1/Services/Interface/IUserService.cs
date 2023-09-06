using Microsoft.AspNetCore.Mvc;
using PharmacyInfrastructure;
using PharmacyInfrastructure.View;

namespace PharmacyWeb.Services.Interface
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUser(RegisterViewModel model);
        Task<UserManagerResponse> LoginUser(LoginViewModel model);
    }
}
