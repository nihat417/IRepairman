using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class AdminController : Controller
	{
		public IActionResult AdminPage()
		{
			return View();
		}
	}
}
