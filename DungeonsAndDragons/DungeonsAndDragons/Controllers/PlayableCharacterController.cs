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
using Newtonsoft.Json.Linq;

namespace DungeonsAndDragons.Controllers
{
    public class PlayableCharacterController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;
        private readonly IHubContext<DnDHub> _hubcontext;

        public PlayableCharacterController(DungeonsAndDragonsContext context, SessionHandler sessionHandler, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _sessionHandler = sessionHandler;
            _hubcontext = hubcontext;
        }



        public IActionResult New(int gamesusersid)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            List<Species> Species = _context.species.ToList();

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.UserID = _sessionHandler.GetSignedInUserID();
            ViewBag.GamesUsersID = gamesusersid;
            ViewBag.Species = Species;
            ViewBag.jsonSpecies = JsonConvert.SerializeObject(Species);

            return View();
        }



        public IActionResult Create(string characterName, int speciesId, int gameUserId = 0)
        {
            int userId = _sessionHandler.GetSignedInUserID();

            GameUser generatedChracter = PlayableCharacter.GenerateCharacter(_context, gameUserId, userId, speciesId, characterName);

            var gameIdQuery = _context.gamesusers.SingleOrDefault(x => x.id == gameUserId);
            var gameId = gameIdQuery.gameid;

            IQueryable gameUserAndPlayableCharacterJoin = Mapping.GameUserAndPlayableCharacterJoin(_context, gameId);

            List<Mapping> acceptedplayers = Game.GetPlayerGames(gameUserAndPlayableCharacterJoin);
            List<Mapping> pendingplayers = Game.GetInvites(gameUserAndPlayableCharacterJoin);

            _hubcontext.Clients.All.SendAsync("UpdatePlayerInvites", JsonConvert.SerializeObject(acceptedplayers), JsonConvert.SerializeObject(pendingplayers));

            return Redirect($"../Game/View/{generatedChracter.gameid}");
        }



        public IActionResult View(int id)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            int characterId = id;

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.UserID = _sessionHandler.GetSignedInUserID();
            ViewBag.Character = PlayableCharacter.GetStatsForUserGeneratedCharacter(_context, characterId);
            ViewBag.Inventory = Inventory.GetPlayersInventoryForDisplay(_context, characterId);
            ViewBag.InventoryJson = JsonConvert.SerializeObject(ViewBag.Inventory);

            return View();
        }



        public IActionResult Use(string json, int itemId, int playableCharacterId)
        {
            //var inventoryJson = JsonConvert.DeserializeObject(json);

            PlayableCharacter.UseHealingItem(_context, itemId, playableCharacterId);
            Inventory.RemoveItemFromInventory(_context, playableCharacterId, itemId, 1);

            return Redirect($"View/{playableCharacterId}");
        }
    }
}