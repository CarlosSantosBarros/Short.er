using Microsoft.AspNetCore.Mvc;
using Short.er.Data;

namespace Short.er.Controllers
{
    public class RedirectController : Controller
    {
        private readonly ApplicationDBContext _db;
        public RedirectController(ApplicationDBContext db)
        {
            _db = db;
        }

        [Route("/{redirectUrl?}")]
        public IActionResult RedirectUrl(string redirectUrl)
        {
            var url = _db.Urls
                .First(entry => entry.Hash == redirectUrl);

            if (url != null) { 
                url.NumberOfRquests++;
                _db.Urls.Update(url);
                _db.SaveChanges();
                return Redirect(url.Url);
            }
            else return Redirect("www.google.com");
        }
    }
}
