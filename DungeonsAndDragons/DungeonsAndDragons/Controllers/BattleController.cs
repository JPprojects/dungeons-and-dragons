using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Hubs;
using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.SignalR;

namespace DungeonsAndDragons.Controllers
{
    public class BattleController : Controller
    {
        private readonly IHubContext<DnDHub> _hubcontext;
        private readonly DungeonsAndDragonsContext _context;

        public BattleController(DungeonsAndDragonsContext context, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _hubcontext = hubcontext;
        }

        public void Create(int gameid, int npcId)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("StartBattleRedirect", gameid, npcId);
        }

        public void End(int gameid)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("EndBattleRedirect", gameid);
        }

        public IActionResult View(int id, int npcId)
        {
            //TODO:
            //Create new instance of Battle - view need to pass this route all required variables
            //Redirect logged out users
            //Redirect users that are not part of this game

            int gameId = id;

            ViewBag.Battle = Battle.StartBattle(_context, gameId, npcId);
            @ViewBag.gameid = id;
            return View();
        }
    }
}
