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
    public class MerchantController : Controller
    {
        private readonly IHubContext<DnDHub> _hubcontext;
        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;

        public MerchantController(DungeonsAndDragonsContext context, IHubContext<DnDHub> hubcontext, SessionHandler sessionHandler)
        {
            _context = context;
            _hubcontext = hubcontext;
            _sessionHandler = sessionHandler;
        }

        public void Create(int gameid)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("StartMerchantRedirect", gameid);
        }

        public void End(int gameid)
        {
            _hubcontext.Clients.Group(gameid.ToString()).SendAsync("EndMerchantRedirect", gameid);
        }

        public IActionResult View(int id)
        {
            //TODO:
            //Redirect users that are not part of this game

            int gameId = id;
            int userid = _sessionHandler.GetSignedInUserID();

            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("../../Home/Index"); }

            if (!Merchant.IsUserInGame(_context, userid, gameId)) { return Redirect("../../Home/Index"); }

            Merchant merchant = Merchant.StartMerchant(_context, gameId);

            List<PlayableCharacter> charactersInGame = merchant.players;

            PlayableCharacter LoggedInUsersCharacter = charactersInGame.Find(x => x.userid == userid);

            List<InventoryItem> merchantWares = _context.inventoryitems.ToList();

            ViewBag.LoggedInUserID = userid;
            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.gameid = gameId;
            ViewBag.Merchant = merchant;

            if (merchant.dmId != userid)
            {
                List<InventoryMapping> inventory = Inventory.GetPlayersInventoryForDisplay(_context, LoggedInUsersCharacter.id);
                ViewBag.Inventory = inventory;
                ViewBag.Balance = inventory.Find(x => x.itemName == "Gold Coins").quantity;
            }

            ViewBag.MerchantWares = merchantWares;


            return View();
        }

        public JsonResult UpdateJSON(string json)
        {
            //var item = json;
            //var deserializedJson = JsonConvert.DeserializeObject<Battle>(json);
            //var gameId = deserializedJson.gameId;

            //Battle.UpdateNpcHp(_context, deserializedJson.NPC.id, deserializedJson.NPC.currentHp);
            //_hubcontext.Clients.Group(deserializedJson.gameId.ToString()).SendAsync("UpdateBattleStats", gameId.ToString(), item);
            ////_hubcontext.Clients.All.SendAsync("UpdateBattleStats", gameId.ToString(), json)
            return Json(json);
        }
    }
}
