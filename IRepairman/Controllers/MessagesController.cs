using IRepairman.Application.ViewModels;
using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
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
