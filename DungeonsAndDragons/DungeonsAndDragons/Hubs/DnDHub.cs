using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DungeonsAndDragons.Hubs
{
    public class DnDHub : Hub
    {
        public async Task UpdatePlayerInvites(string acceptedplayers, string pendingplayers)
        {
            await Clients.All.SendAsync("UpdatePlayerInvites", acceptedplayers, pendingplayers);
        }
    }
}