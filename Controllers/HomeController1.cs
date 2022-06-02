using Microsoft.AspNetCore.Mvc;

namespace Short.er.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
