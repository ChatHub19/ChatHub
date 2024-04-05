using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Hubs
{

    public class MessageHub : Hub
    {
        private static readonly Dictionary<string, string[]> _users = new();

        public async Task SendJoinedMessageToAll()
        {
            var role = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            var group = Context.User?.Claims.FirstOrDefault(c => c.Type == "Group")?.Value;
            await Clients.All.SendAsync("ReceiveJoinedMessage", 
                $"{role} {Context?.User?.Identity?.Name} in Group {group} joined");
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
            var username = Context?.User?.Identity?.Name;
            var connectionid = Context?.ConnectionId;
            var userGuid = Context?.UserIdentifier;
            if (username is not null && connectionid is not null && userGuid is not null && !_users.ContainsKey(username))
            {
                _users.Add(username, new string[] { connectionid, userGuid });
            }
            await Clients.All.SendAsync("ReceiveConnectedUsers", _users);
        }
    }
}
