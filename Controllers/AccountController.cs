using System.Diagnostics;
using System.Security.Claims;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Short.er.Models;

namespace Short.er.Controllers {
	public class AccountController : Controller {
		private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<CognitoUser> _signInManager;
		private readonly CognitoUserManager<CognitoUser> _userManager;
		private readonly CognitoUserPool _pool;

		public AccountController(
				ILogger<AccountController> logger,
				SignInManager<CognitoUser> signInManager,
				UserManager<CognitoUser> userManager,
				CognitoUserPool pool) {
			_logger = logger;
			_signInManager = signInManager;
			_userManager = userManager as CognitoUserManager<CognitoUser>;
			_pool = pool;
		}

		public IActionResult Login() => View();

		[Route("/Account/Login")]
		[HttpPost]
		public async Task<IActionResult> Login(LoginModel formData) {
			if (ModelState.IsValid) {
				var result = await _signInManager.PasswordSignInAsync(formData.UserName, formData.Password, formData.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded) {
					_logger.LogInformation("User logged in.");
					return RedirectToAction("Index", "Root");
				} else if (result.IsCognitoSignInResult()) {
					if (result is CognitoSignInResult cognitoResult) {
						if (cognitoResult.RequiresPasswordChange) {
							_logger.LogWarning("User password needs to be changed");
							return RedirectToAction("./ChangePassword");
						} else if (cognitoResult.RequiresPasswordReset) {
							_logger.LogWarning("User password needs to be reset");
							return RedirectToAction("./ResetPassword");
						}
					}
				} else {
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
					return View();
				}
			}

			// If we got this far, something failed, redisplay form
			return RedirectToAction("Index", "Root");
		}

		public IActionResult Register() => View();

		[Route("/Account/Register")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel formData) {

			if (ModelState.IsValid) {
				var user = _pool.GetUser(formData.Email);
				var result = await _userManager.CreateAsync(user, formData.Password);

				if (result.Succeeded) {
					_logger.LogInformation("User created a new account with password.");

					await _signInManager.SignInAsync(user, isPersistent: false);

					return RedirectToAction("ConfirmAccount");
				}
				foreach (var error in result.Errors) {
					Debug.WriteLine(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return RedirectToAction("Index", "Root");
		}

		public IActionResult ConfirmAccount() => View();

		[Route("/Account/ConfirmAccount")]
		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> ConfirmAccount(ConfirmAccountModel formData) {

			if (ModelState.IsValid) {
				string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

				CognitoUser user = await _userManager.FindByIdAsync(userId);
				if (user == null) {
					return NotFound($"Unable to load user with ID '{userId}'.");
				}

				var result = await _userManager.ConfirmSignUpAsync(user, formData.Code, true);
				if (!result.Succeeded) {
					throw new InvalidOperationException($"Error confirming account for user with ID '{userId}':");
				}
			}

			// If we got this far, something failed, redisplay form
			return RedirectToAction("Index", "Root");
		}

		public async Task<IActionResult> Logout() {
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");

			return RedirectToAction("Index", "Root");
		}

		// paste above here
	}
}

