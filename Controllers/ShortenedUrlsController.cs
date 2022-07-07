using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Short.er.Data;
using Short.er.Models;
using Short.er.Utils;

namespace Short.er.Controllers {
	public class ShortenedUrlsController : Controller {
		private readonly ApplicationDBContext _context;
		private readonly CognitoUserManager<CognitoUser> _userManager;

		public ShortenedUrlsController(ApplicationDBContext context,
		UserManager<CognitoUser> userManager) {
			_context = context;
			_userManager = userManager as CognitoUserManager<CognitoUser>;
		}

		// GET: ShortenedUrls
		[Route("/ShortenedUrls")]
		public async Task<IActionResult> Index() {
			string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

			List<ShortenedUrl> content = await _context.Urls
					.Where(entry => entry.UserId
					== userId)
					.ToListAsync();

			return _context.Urls != null ?
						View(content) :
						Problem("Entity set 'ApplicationDBContext.Urls'  is null.");
		}

		// GET: ShortenedUrls/Details/5
		public async Task<IActionResult> Details(int? id) {
			if (id == null || _context.Urls == null) {
				return NotFound();
			}

			var shortenedUrl = await _context.Urls
				.FirstOrDefaultAsync(m => m.Id == id);
			if (shortenedUrl == null) {
				return NotFound();
			}

			return View(shortenedUrl);
		}

		// GET: ShortenedUrls/Create

		public IActionResult Create() {
			return View();
		}

		// POST: ShortenedUrls/Create
		[Route("/ShortenedUrls/Create")]
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ShortenedUrl shortenedUrl) {
			string input = StringUtils.RemoveBadChars(shortenedUrl.Url);
			SHA1 hasher = SHA1.Create();
			string hash = HashUtils.GetHash(hasher, input);
			/*			Debug.WriteLine($"The SHA256 hash of {input} {input.Length} is: {hash} {hash.Length}.");
						Debug.WriteLine("Verifying the hash...");
						if (HashUtils.VerifyHash(hasher, input, hash)) {
							Debug.WriteLine("The hashes are the same.");
						} else {
							Debug.WriteLine("The hashes are not same.");
						}*/
			string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

			shortenedUrl.Hash = hash;
			shortenedUrl.CreatedDateTime = DateTime.Now;
			shortenedUrl.NumberOfRquests = 0;
			shortenedUrl.UserId = userId;

			Debug.Write(userId);

			TryValidateModel(shortenedUrl);
			if (ModelState.IsValid) {
				_context.Add(shortenedUrl);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(shortenedUrl);
		}

		// GET: ShortenedUrls/Edit/5
		public async Task<IActionResult> Edit(int? id) {
			if (id == null || _context.Urls == null) {
				return NotFound();
			}

			var shortenedUrl = await _context.Urls.FindAsync(id);
			if (shortenedUrl == null) {
				return NotFound();
			}
			return View(shortenedUrl);
		}

		// POST: ShortenedUrls/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Url,Hash,CreatedDateTime,NumberOfRquests")] ShortenedUrl shortenedUrl) {
			if (id != shortenedUrl.Id) {
				return NotFound();
			}

			if (ModelState.IsValid) {
				try {
					_context.Update(shortenedUrl);
					await _context.SaveChangesAsync();
				} catch (DbUpdateConcurrencyException) {
					if (!ShortenedUrlExists(shortenedUrl.Id)) {
						return NotFound();
					} else {
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(shortenedUrl);
		}

		// GET: ShortenedUrls/Delete/5
		public async Task<IActionResult> Delete(int? id) {
			if (id == null || _context.Urls == null) {
				return NotFound();
			}

			var shortenedUrl = await _context.Urls
				.FirstOrDefaultAsync(m => m.Id == id);
			if (shortenedUrl == null) {
				return NotFound();
			}

			return View(shortenedUrl);
		}

		// POST: ShortenedUrls/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id) {
			if (_context.Urls == null) {
				return Problem("Entity set 'ApplicationDBContext.Urls'  is null.");
			}
			var shortenedUrl = await _context.Urls.FindAsync(id);
			if (shortenedUrl != null) {
				_context.Urls.Remove(shortenedUrl);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ShortenedUrlExists(int id) {
			return (_context.Urls?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
