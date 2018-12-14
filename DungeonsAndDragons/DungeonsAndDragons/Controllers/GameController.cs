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

namespace DungeonsAndDragons.Controllers
{
    public class GameController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;

        public GameController(DungeonsAndDragonsContext context, SessionHandler sessionHandler)
        {
            _context = context;
            _sessionHandler = sessionHandler;
        }

        public IActionResult Index()
        {
            if (!_sessionHandler.UserIsSignedIn()) {Response.Redirect("/");}

            int userid = HttpContext.Session.GetInt32("userID") ?? default(int);
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            @ViewBag.DMGames = _context.games.Where(x => x.dm == userid);

            IQueryable games =
               from gameuser in _context.gamesusers
               join game in _context.games
               on gameuser.gameid equals game.id
               join user in _context.users
               on game.dm equals user.id
               where userid == gameuser.userid
               select new Mapping
               {
                   id = gameuser.id,
                   gameid = game.id,
                   gamename = game.name,
                   gamedm = game.dm,
                   playablecharacterid = gameuser.playablecharacterid,
                   userid = user.id,
                   userusername = user.username
               };

            List<Mapping> playergames = new List<Mapping>();
            List<Mapping> gameinvites = new List<Mapping>();

            foreach (Mapping game in games)
            {
                if (game.playablecharacterid != null)
                {
                    playergames.Add(game);
                }
                else
                {
                    gameinvites.Add(game);
                }
            }

            @ViewBag.PlayerGames = playergames;
            @ViewBag.Invites = gameinvites;

            return View();
        }

        public IActionResult New()
        {
            if (!_sessionHandler.UserIsSignedIn()){return Redirect("Home/Index");}

            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            return View();
        }

        public IActionResult Create(string name)
        {
            if (!_sessionHandler.UserIsSignedIn()){return Redirect("Home/Index");}

            int user_id = HttpContext.Session.GetInt32("userID") ?? default(int);
            var game = new Game { name = name, dm = user_id };
            _context.games.Add(game);
            _context.SaveChanges();

            return Redirect($"View/{game.id}");
        }

        public IActionResult View(int id)
        {
            if (!_sessionHandler.UserIsSignedIn()) { return Redirect("Home/Index"); }

            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            var game = _context.games.SingleOrDefault(x => x.id == id);

            var dm_id = game.dm;
            ViewBag.DM = _context.users.SingleOrDefault(x => x.id == dm_id);

            IQueryable gameusers =
               from gameuser in _context.gamesusers
               join user in _context.users
               on gameuser.userid equals user.id
               where gameuser.gameid == id
               select new Mapping
               {
                   userid = user.id,
                   userusername = user.username,
                   playablecharacterid = gameuser.playablecharacterid,
               };

            List<Mapping> ingameusers = new List<Mapping>();
            List<Mapping> invitedusers = new List<Mapping>();

            foreach (Mapping user in gameusers)
            {
                if (user.playablecharacterid != null)
                {
                    ingameusers.Add(user);
                }
                else
                {
                    invitedusers.Add(user);
                }
            }
            ViewBag.Users = ingameusers;
            ViewBag.PendingUsers = invitedusers;

            ViewBag.Game = game;
            ViewBag.Message = TempData["FlashMessage"];
            return View();
        }

        public IActionResult Invite(int id, string username)
        {
            var inviteduser = _context.users.SingleOrDefault(x => x.username == username);

            if (inviteduser == null)
            {
                TempData["FlashMessage"] = "Player does not exist";
            }
            else if (username == HttpContext.Session.GetString("username"))
            {
                TempData["FlashMessage"] = "Cannot invite yourself to a game";
            }
            else if (_context.gamesusers.SingleOrDefault(x => x.userid == inviteduser.id & x.gameid == id) != null)
            {
                TempData["FlashMessage"] = "Player has already been invited";
            }
            else
            {
                _context.gamesusers.Add(new GameUser { gameid = id, userid = inviteduser.id });
                _context.SaveChanges();
            }

            return Redirect($"View/{id}");
        }

        public IActionResult Decline(int id)
        {
            var gameuser = _context.gamesusers.SingleOrDefault(x => x.id == id);

            _context.gamesusers.Remove(gameuser);
            _context.SaveChanges();

            return Redirect("../Game");
        }
    }
}