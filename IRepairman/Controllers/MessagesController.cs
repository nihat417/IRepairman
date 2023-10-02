using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
    public class MessagesController : Controller
	{
		public IActionResult Messages()
		{
			return View();
		}
	}
}
