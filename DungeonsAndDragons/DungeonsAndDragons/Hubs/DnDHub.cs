using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DungeonsAndDragons.Hubs
{
    public class DnDHub : Hub
    {
        public async Task AcceptInvite(int gameUserId)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}