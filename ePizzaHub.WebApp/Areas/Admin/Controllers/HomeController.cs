using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.WebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
