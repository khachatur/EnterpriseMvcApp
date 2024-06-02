using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using EnterpriseMvcApp.Data;
using EnterpriseMvcApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EnterpriseMvcApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseMvcApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly ILogger<UserController> _logger;

		public UserController(IUserService userService, ILogger<UserController> logger)
		{
			_userService = userService;
			_logger = logger;
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var user = new User
					{
						Id = Guid.NewGuid(),
						Username = model.Username,
						Email = model.Email,
						PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
						CreatedAt = DateTime.UtcNow,
						UpdatedAt = DateTime.UtcNow,
						Role = "User" // Default role for new users
					};

					await _userService.AddUserAsync(user);
					_logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);
					return RedirectToAction(nameof(Login));
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error registering user");
					ModelState.AddModelError("", "An error occurred while registering the user. Please try again.");
				}
			}
			return View(model);
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userService.GetUserByUsernameOrEmailAsync(model.UsernameOrEmail);
				if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
				{
					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.Username),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, user.Role)
				};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var authProperties = new AuthenticationProperties
					{
						IsPersistent = true
					};

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

					return RedirectToAction(nameof(Profile));
				}
				ModelState.AddModelError("", "Invalid login attempt.");
			}
			return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Profile()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));

			if (user == null)
			{
				return NotFound();
			}

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction(nameof(Login));
		}
	}
}
