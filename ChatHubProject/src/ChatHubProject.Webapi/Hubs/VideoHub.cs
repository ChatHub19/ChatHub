using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Hubs
{
    public class VideoHub : Hub
    {
        public async Task SendVideoCallToAll(string data)
        {
            await Clients.Others.SendAsync("ReceiveVideoData", data);
        }
    }
}
