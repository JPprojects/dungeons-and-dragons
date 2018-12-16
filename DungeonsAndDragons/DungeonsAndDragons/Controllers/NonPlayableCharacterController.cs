using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Models;
using DungeonsAndDragons.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using StaticHttpContextAccessor.Helpers;

namespace DungeonsAndDragons.Controllers
{
    public class NonPlayableCharacterController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;
        private readonly IHubContext<DnDHub> _hubcontext;

        public NonPlayableCharacterController(DungeonsAndDragonsContext context, SessionHandler sessionHandler, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _sessionHandler = sessionHandler;
            _hubcontext = hubcontext;
        }



        public IActionResult New(int gameid, int dmid)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.GameID = gameid;
            ViewBag.Species = _context.species.ToList();

            return View();
        }



        public IActionResult Create(string characterName, int gameId, int speciesId)
        {
            NonPlayableCharacter.GenerateNPC(_context, gameId, speciesId, characterName);

            return Redirect($"../Game/View/{gameId}");
        }



        public IActionResult View(int id)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            int characterId = id;

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.Character = NonPlayableCharacter.GetStatsForNpcCharacter(_context, characterId);

            return View();
        }
    }
}
