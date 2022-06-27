using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Short.er.Models;

namespace Short.er.Controllers {
  public class AccountController : Controller {
	private readonly SignInManager<CognitoUser> _signInManager;
	private readonly ILogger<AccountController> _logger;

	public AccountController(SignInManager<CognitoUser> signInManager, ILogger<AccountController> logger) {
	  _signInManager = signInManager;
	  _logger = logger;
	}

	public IActionResult Login() {
	  return View();
	}

	[Route("/Account/Login")]
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginModel formData) {


	  if (ModelState.IsValid) {
		var result = await _signInManager.PasswordSignInAsync(formData.UserName, formData.Password, formData.RememberMe, lockoutOnFailure: false);
		if (result.Succeeded) {
		  _logger.LogInformation("User logged in.");
		  // ridirect here
		}
		else if (result.RequiresTwoFactor) {
		  return RedirectToAction("./LoginWith2fa", new { ReturnUrl = formData.ReturnUrl, RememberMe = formData.RememberMe });
		}
		else if (result.IsCognitoSignInResult()) {
		  if (result is CognitoSignInResult cognitoResult) {
			if (cognitoResult.RequiresPasswordChange) {
			  _logger.LogWarning("User password needs to be changed");
			  return RedirectToAction("./ChangePassword");
			}
			else if (cognitoResult.RequiresPasswordReset) {
			  _logger.LogWarning("User password needs to be reset");
			  return RedirectToAction("./ResetPassword");
			}
		  }

		}

		ModelState.AddModelError(string.Empty, "Invalid login attempt.");
		return RedirectToAction(nameof(Index));
	  }

	  // If we got this far, something failed, redisplay form
	  return RedirectToAction(nameof(Index));
	}

	IActionResult RedirectToPageResult(string v, object value) => throw new NotImplementedException();
  }
}


