using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.WebApp.Areas.User.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
