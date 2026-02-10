using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
