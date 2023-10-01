using IRepairman.Application.ViewModels;
using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.Mvc;

namespace IRepairman.Controllers
{
	public class MessagesController : Controller
	{
		private readonly AppDbContext _context;

		public MessagesController(AppDbContext context)
		{
			_context = context;
		}
		public IActionResult Messages()
		{
			//var receiverId = "23ed089a-ad11-4052-8bf6-2383b9b372bc";
			//
			//var model = new MessageVm
			//{
			//	ReceiverId = receiverId
			//};
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(string senderId, string receiverId, string content)
		{
			if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(content))
			{
				return BadRequest("Invalid input data.");
			}

			var message = new MessageContact
			{
				SenderId = senderId,
				ReceiverId = receiverId,
				Content = content,
				SentAt = DateTime.Now
			};

			_context.messages.Add(message);
			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
