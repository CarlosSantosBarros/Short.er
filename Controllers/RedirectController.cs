using Microsoft.AspNetCore.Mvc;

namespace Short.er.Controllers
{
    public class RedirectController : Controller
    {
        [Route("/{redirectUrl?}")]
        public IActionResult RedirectUrl(string? redirectUrl)
        {
            if (redirectUrl == "test")  return Redirect("https://www.archlinux.org");
            // datatbase query useing redirectUrl
            // return redirect url
            else return Redirect("https://www.google.com");
        }
    }
}
