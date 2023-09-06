using Microsoft.AspNetCore.Mvc;
using PharmacyDB.Interfaces;

namespace PharmacyAdminWebApp.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
