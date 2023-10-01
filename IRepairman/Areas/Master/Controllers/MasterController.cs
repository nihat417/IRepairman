using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Areas.Master.Controllers
{
	[Area("Master")]
	[Authorize(Roles = "Master")]
	[Authorize]
	public class MasterController : Controller
	{
		private readonly AppDbContext appDbContext;
		public MasterController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult MasterPage()
		{
			var masters = appDbContext.masters.FirstOrDefault();
			return View(masters);
		}
	}
}
