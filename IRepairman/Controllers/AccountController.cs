using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet]
		public IActionResult Login(string ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			return View();
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
	}
}
