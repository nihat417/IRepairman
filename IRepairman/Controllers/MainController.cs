using IRepairman.Helpers;
using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
	[Authorize]
	public class MainController : Controller
	{
		private readonly AppDbContext context;
		private readonly IHttpContextAccessor httpContextAccessor;

		public MainController(AppDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			this.context = context;
			this.httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Index()
		{
			var masters = context.masters.ToList();
			return View(masters);
		}

		public IActionResult Favorites()
		{
			var favoriteMasters = HttpContext.Session.Get<List<string>>("FavoriteMasters") ?? new List<string>();
			var favoriteMasterDetails = context.masters.Where(m => favoriteMasters.Contains(m.Id)).ToList();

			return View(favoriteMasterDetails);
		}


		[HttpPost]
		public IActionResult AddFavorite(string id)
		{
			var masters = context.masters.Find(id);
			if (masters != null)
			{
				var favoriteMasters = HttpContext.Session.Get<List<object>>("FavoriteMasters") ?? new List<object>();
				favoriteMasters.Add(id);

				HttpContext.Session.Set("FavoriteMasters", favoriteMasters);
                var refererUrl = HttpContext.Request.Headers["Referer"].ToString();
                return Redirect(refererUrl);
            }

			return NotFound();
		}


	}
}
