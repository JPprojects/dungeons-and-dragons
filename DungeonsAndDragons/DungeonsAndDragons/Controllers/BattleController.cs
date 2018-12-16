using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Hubs;
using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.SignalR;
using StaticHttpContextAccessor.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DungeonsAndDragons.Controllers
{
    public class BattleController : Controller
    {
        private readonly IHubContext<DnDHub> _hubcontext;
        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;

        public BattleController(DungeonsAndDragonsContext context, IHubContext<DnDHub> hubcontext, SessionHandler sessionHandler)
        {
            _context = context;
            _hubcontext = hubcontext;
            _sessionHandler = sessionHandler;
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

        public IActionResult View(int id)
        {
            //TODO:
            //Create new instance of Battle - view need to pass this route all required variables
            //Redirect logged out users
            //Redirect users that are not part of this game


            ViewBag.NPC = _context.nonplayablecharacters.SingleOrDefault(x => x.id == 1);
            ViewBag.Players = _context.playablecharacters.Where(x => x.userid == 1);
            ViewBag.GameDMID = 1;
            ViewBag.UserID = _sessionHandler.GetSignedInUserID();


            //@ViewBag.Battle = npc;
            @ViewBag.GameID = id;
            return View();
        }
    }
}
