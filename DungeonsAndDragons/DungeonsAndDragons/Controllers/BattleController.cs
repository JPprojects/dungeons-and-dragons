using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Hubs;
using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        public void Create(int gameid, int npc)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("StartBattleRedirect", gameid);

            //return RedirectToAction($"View/{gameid}", new { gameid = gameid, npc = npc });
        }

        public IActionResult End(int gameid)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("EndBattleRedirect", gameid);
            return Redirect($"../Game/View/{gameid}");
        }

        public IActionResult View(int id, int npc)
        {
            _hubcontext.Clients.Group(id.ToString()).SendAsync("StartBattleRedirect", id);
            //TODO:
            //Create new instance of Battle - view need to pass this route all required variables
            //Redirect logged out users
            //Redirect users that are not part of this game
            @ViewBag.Battle = npc;
            @ViewBag.GameID = id;
            return View();
        }
    }
}
