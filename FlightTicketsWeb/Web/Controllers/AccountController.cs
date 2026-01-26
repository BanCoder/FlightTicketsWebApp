using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightTicketsWeb.Core.Interfaces;
using FlightTicketsWeb.Web.ViewModels;
using FlightTicketsWeb.Web.ViewModels.Persistence;

namespace FlightTicketsWeb.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAuthService _authService;
		private readonly FlightTicketsContext _context;
		private readonly ILogger<AccountController> _logger; 
		public AccountController(IAuthService authService, FlightTicketsContext context, ILogger<AccountController> logger)
		{
			_authService = authService;
			_context = context;
			_logger = logger;
		}
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Entrance()
		{
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Entrance(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await _authService.AuthenticateAsync(model.Email, model.Password);

			if (user == null)
			{
				ModelState.AddModelError("", "Неверный email или пароль");
				return View(model);
			}
			await _authService.SignInAsync(HttpContext, user);
			_logger.LogInformation("Выполнился вход пользователя в аккаунт");
			TempData["ShowAlert"] = true;
			TempData["SuccessMessage"] = "Успешный вход в аккаунт!";
			if (_authService.IsAdmin(user))
			{
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Registration()
		{
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Registration(RegisterModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			if (model.Password != model.ConfirmPassword)
			{
				ModelState.AddModelError("ConfirmPassword", "Пароли не совпадают");
				return View(model);
			}

			try
			{
				var user = await _authService.RegisterAsync(
					model.Email,
					model.Password,
					model.FirstName,
					model.LastName,
					"user");
				await _authService.SignInAsync(HttpContext, user);
				_logger.LogInformation("Выполнилась регистрация пользователя");
				TempData["ShowAlert"] = true;
				TempData["SuccessMessage"] = "Регистрация прошла успешно!";
				return RedirectToAction("Index", "Home");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _authService.SignOutAsync(HttpContext);
			_logger.LogInformation("Выполнился выход пользователя из аккаунта");
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}
		public IActionResult Promotions()
		{
			return View();
		}
	}
}
