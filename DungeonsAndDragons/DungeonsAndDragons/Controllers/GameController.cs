using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Models;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using StaticHttpContextAccessor.Helpers;
using Microsoft.AspNetCore.SignalR;
using DungeonsAndDragons.Hubs;

namespace DungeonsAndDragons.Controllers
{
    public class GameController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;
        private readonly IHubContext<DnDHub> _hubcontext;

        public GameController(DungeonsAndDragonsContext context, SessionHandler sessionHandler, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _sessionHandler = sessionHandler;
            _hubcontext = hubcontext;
        }



        public IActionResult Index()
        {
            if (!_sessionHandler.UserIsSignedIn()) { Response.Redirect("/"); }

            int signedInUserId = _sessionHandler.GetSignedInUserID();
            IQueryable gameAndUserJoin = Mapping.GameAndUserJoin(_context, signedInUserId);

            ViewBag.UserId = signedInUserId;
            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.DMGames = Game.GetDMGames(_context, signedInUserId);
            ViewBag.AcceptedGames = Game.GetPlayerGames(gameAndUserJoin);
            ViewBag.PendingGames = Game.GetInvites(gameAndUserJoin);

            return View();
        }



        public IActionResult New()
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.UserID = _sessionHandler.GetSignedInUserID();

            return View();
        }



        public IActionResult Create(string gameName, int signedInUserId)
        {
            Game game = Game.CreateGame(_context, gameName, signedInUserId);

            return Redirect($"View/{game.id}");
        }



        public IActionResult View(int id)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            int gameId = id;

            Game game = Game.getGameById(_context, gameId);
            IQueryable gameLobbyAcceptedAndPendingPlayers = Mapping.GameUserAndPlayableCharacterJoin(_context, gameId);

            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.UserID = _sessionHandler.GetSignedInUserID();
            ViewBag.NPCs = _context.nonplayablecharacters.Where(x => x.gameid == gameId);
            ViewBag.AcceptedUsers = Game.GetPlayerGames(gameLobbyAcceptedAndPendingPlayers);
            ViewBag.PendingUsers = Game.GetInvites(gameLobbyAcceptedAndPendingPlayers);
            ViewBag.Game = game;
            ViewBag.Message = TempData["FlashMessage"];
            ViewBag.DM = _context.users.SingleOrDefault(x => x.id == game.dm);
            return View();
        }



        public IActionResult Invite(int id, string inviteeUsername)
        {
            var inviteduser = _context.users.SingleOrDefault(x => x.username == inviteeUsername);

            if (inviteduser == null)
            {
                TempData["FlashMessage"] = "Player does not exist.";
            }
            else if (inviteeUsername == HttpContext.Session.GetString("username"))
            {
                TempData["FlashMessage"] = "Cannot invite yourself to a game.";
            }
            else if (_context.gamesusers.SingleOrDefault(x => x.userid == inviteduser.id & x.gameid == id) != null)
            {
                TempData["FlashMessage"] = "Player has already been invited.";
            }
            else
            {
                _context.gamesusers.Add(new GameUser { gameid = id, userid = inviteduser.id });
                _context.SaveChanges();
            }
            string invitedUserId = inviteduser.id.ToString();
            string playerGroup = "Player-" + invitedUserId;

            _hubcontext.Clients.Group(playerGroup).SendAsync("UpdateUserInvites", playerGroup);

            return Redirect($"View/{id}");
        }



        public IActionResult Decline(int id)
        {
            int gameUserId = id;

            Game.DeclineInvite(_context, gameUserId);

            return Redirect("../Game");
        }
    }
}