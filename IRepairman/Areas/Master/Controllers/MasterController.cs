using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Areas.Master.Controllers
{
	[Area("Master")]
	[Authorize]
	public class MasterController : Controller
	{
		public IActionResult MasterPage()
		{
			return View();
		}
	}
}
