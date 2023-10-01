using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.SignalR;

namespace IRepairman.Persistence.Services
{
	public class ChatHub : Hub
	{
		private readonly AppDbContext _context;

		public ChatHub(AppDbContext context)
		{
			_context = context;
		}

		public async Task SendMessage(string senderId, string receiverId, string content)
		{
			var message = new MessageContact
			{
				SenderId = senderId,
				ReceiverId = receiverId,
				Content = content,
				SentAt = DateTime.Now
			};

			_context.messages.Add(message);
			await _context.SaveChangesAsync();

			await Clients.All.SendAsync("ReceiveMessage", senderId, content);
		}
	}

}
