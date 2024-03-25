using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Hubs
{

    public class MessageHub : Hub
    {
        private readonly List<string?> _users = new();

        public async Task SendJoinedMessageToAll()
        {
            var role = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            var group = Context.User?.Claims.FirstOrDefault(c => c.Type == "Group")?.Value;
            await Clients.All.SendAsync("ReceiveJoinedMessage", 
                $"{role} {Context?.User?.Identity?.Name} in Group {group} joined");

            _users.Add(Context?.User?.Identity?.Name);
        }

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

        public async Task RequestConnectedUsers()
        {
            await Clients.Caller.SendAsync("ReceiveConnectedUsers", _users);
        }
    }
}
