using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Hubs;
using DungeonsAndDragons.Models;
using StaticHttpContextAccessor.Helpers;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;

namespace DungeonsAndDragons.Hubs
{
    public class DnDHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Player-" + userId);
        }

        public async Task UpdateUserInvites(string playerGroup)
        {
            await Clients.Group(playerGroup).SendAsync("UpdateUserInvites");
        }

        public async Task UpdatePlayerInvites(string acceptedplayers, string pendingplayers)
        {
            await Clients.All.SendAsync("UpdatePlayerInvites", acceptedplayers, pendingplayers);
        }

        public async Task StartBattleRedirect(int gameid, int npcId)
        {
            await Clients.Group(gameid.ToString()).SendAsync("StartBattleRedirect", gameid, npcId);
        }

        public async Task EndBattleRedirect(int gameid)
        {
            await Clients.Group(gameid.ToString()).SendAsync("EndBattleRedirect", gameid);
        }

        public async Task JoinGame(string gameid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameid);
        }

        public async Task UpdateBattleStats(string gameid, string updatedStatsJson)
        {
            await Clients.Group(gameid).SendAsync("UpdateBattleStats", gameid, updatedStatsJson);

        }

        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.Remove(Context.ConnectionId, roomName);
        //}

        public async Task StartMerchantRedirect(int gameid)
        {
            await Clients.Group(gameid.ToString()).SendAsync("StartMerchantRedirect", gameid);
        }

        public async Task EndMerchantRedirect(int gameid)
        {
            await Clients.Group(gameid.ToString()).SendAsync("EndMerchantRedirect", gameid);
        }
    }
}
