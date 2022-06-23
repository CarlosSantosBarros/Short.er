using Microsoft.AspNetCore.Mvc;
using Short.er.Data;
using System.Diagnostics;

namespace Short.er.Controllers {
  public class RedirectController : Controller {

	private readonly ApplicationDBContext _db;
	public RedirectController(ApplicationDBContext db) {
	  _db = db;
	}

	[Route("/{redirectUrl?}")]
	public IActionResult RedirectUrl(string redirectUrl) {

	  var url = _db.Urls.SingleOrDefault(entry => entry.Hash.StartsWith(redirectUrl));
	  Debug.WriteLine(redirectUrl, "Url Data");

	  if (url != null) {
		url.NumberOfRquests++;
		_db.Urls.Update(url);
		_db.SaveChanges();
		return Redirect(url.Url);
	  }
	  else
		return NotFound();
	}
  }
}