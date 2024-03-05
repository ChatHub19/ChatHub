using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Hubs
{

    public class MessageHub : Hub
    {
        public async Task SendMessageToAll(string text, string displayname, string time)
        {
            await Clients.All.SendAsync("ReceiveMessage", text, displayname, time);
        }

        public async Task SendMessageToCaller(string text, string displayname, string time)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", text, displayname, time);
        }

        public async Task SendMessageToGroup(string text, string displayname, string time)
        {
            await Clients.Group("SignalR USers").SendAsync("ReceiveMessage", text, displayname, time);
        }
    }
}
