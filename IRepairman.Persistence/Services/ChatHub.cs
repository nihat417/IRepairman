using IRepairman.Domain.Entities;
using IRepairman.Persistence.Datas;
using Microsoft.AspNetCore.SignalR;

namespace IRepairman.Persistence.Services
{
	public class ChatHub : Hub
	{
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }

}
