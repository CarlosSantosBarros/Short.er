using Microsoft.AspNetCore.Mvc;
using Short.er.Models;
using System.Diagnostics;

namespace Short.er.Controllers {
  public class RootController : Controller {

    private readonly ILogger<RootController> _logger;

    public RootController(ILogger<RootController> logger) {
      _logger = logger;
    }
    [Route("")]
    public IActionResult Index() {
      return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}