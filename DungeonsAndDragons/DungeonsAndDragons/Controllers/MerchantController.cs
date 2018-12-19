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

            ViewBag.Message = TempData["FlashMessage"];
            ViewBag.LoggedInUserID = userid;
            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.gameid = gameId;
            ViewBag.Merchant = merchant;

            //TODO
            //Add player name to view

            if (merchant.dmId != userid)
            {
                ViewBag.player = LoggedInUsersCharacter;
                List<InventoryMapping> inventory = Inventory.GetPlayersInventoryForDisplay(_context, LoggedInUsersCharacter.id);
                ViewBag.Inventory = inventory;
                ViewBag.Balance = inventory.Find(x => x.itemName == "Gold Coins").quantity;
            }

            ViewBag.MerchantWares = merchantWares;


            return View();
        }

        public IActionResult Purchase(int gameId, int itemId, int characterId)
        {
            List<Inventory> playersInventory = Inventory.getPlayersInventory(_context, characterId);

            int characterBalance = playersInventory.Find(item => item.inventoryItemId == 6).quantity;

            int itemPrice = _context.inventoryitems.ToList().Find(item => item.id == itemId).monetaryValue;

            if (characterBalance < itemPrice)
            {

                TempData["FlashMessage"] = "Insufficent funds.";

            }
            else
            {
                Inventory.RemoveItemFromInventory(_context, characterId, 6, itemPrice);

                Inventory.addItemToInventory(_context, characterId, itemId, 1);
            }

            return Redirect($"View/{gameId}");
        }
    }
}
