using IRepairman.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class AdminController : Controller
	{
		private readonly IUserRepository userRepository;

        public AdminController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

		[HttpGet]
        public async Task<IActionResult> AdminPage()
		{
			var users = await userRepository.GetAllUsersAsync();
			ViewBag.Users = users;
			return View();
		}

		[HttpGet]
		public IActionResult Specializations()
		{
			return View();
		}
	}
}
