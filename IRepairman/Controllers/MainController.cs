using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
	[Authorize]
	public class MainController : Controller
	{
		private readonly AppDbContext context;

		public MainController(AppDbContext context)
		{
			this.context = context;
		}

		public IActionResult Index()
		{
			var masters = context.masters.ToList();
			return View(masters);
		}
	}
}
