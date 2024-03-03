using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Hubs
{
    public class MessageHub : Hub
    {
        public async Task ReceiveMessage(string message, string displayname, string time)
        {
            await Clients.All.SendAsync("ReceiveMessage", new { Message = message, Displayname = displayname, Time = time });
        }
    }
}
