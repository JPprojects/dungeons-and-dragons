using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Hubs;
using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.SignalR;
using StaticHttpContextAccessor.Helpers;
using Newtonsoft.Json;

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
            //Redirect users that are not part of this game

            int gameId = id;

            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("../../Home/Index"); }

            if (!Battle.IsUserInGame(_context, _sessionHandler.GetSignedInUserID(), gameId)) { return Redirect("../../Home/Index"); }

            Battle battle = Battle.StartBattle(_context, gameId, npcId);

            if (battle.players.Count == 0) { TempData["FlashMessage"] = "No players available."; return Redirect($"../../Game/View/{gameId}"); }

            ViewBag.Battle = battle;
            ViewBag.jsonBattle = JsonConvert.SerializeObject(battle);
            ViewBag.gameid = id;
            ViewBag.LoggedInUserID = _sessionHandler.GetSignedInUserID();
            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            return View();
        }

        public JsonResult UpdateJSON(string json)
        {
            var item = json;
            var deserializedJson = JsonConvert.DeserializeObject<Battle>(json);
            var gameId = deserializedJson.gameId;

            Battle.UpdateNpcHp(_context, deserializedJson.NPC.id, deserializedJson.NPC.currentHp);
            _hubcontext.Clients.Group(deserializedJson.gameId.ToString()).SendAsync("UpdateBattleStats", gameId.ToString(), item);
            //_hubcontext.Clients.All.SendAsync("UpdateBattleStats", gameId.ToString(), json)
            return Json(json);
        }
    }
}
