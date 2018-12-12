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

namespace DungeonsAndDragons.Controllers
{
    public class GameController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;

        public GameController(DungeonsAndDragonsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                Response.Redirect("/");
            }
            int userid = HttpContext.Session.GetInt32("userID") ?? default(int);
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            @ViewBag.DMGames = _context.games.Where(x => x.dm == userid);

            var games =
               from gameuser in _context.gamesusers
               join game in _context.games
               on gameuser.gameid equals game.id where gameuser.userid == userid
               select new Game
               {
                   id = game.id,
                   name = game.name,
                   dm = game.dm,
               };
            games.ToList();
            @ViewBag.PlayerGames = games;

            var invites =
               from gameuser in _context.gamesusers
               join game in _context.games
               on gameuser.gameid equals game.id
               where gameuser.userid == userid
               select new Game
               {
                   id = game.id,
                   name = game.name,
                   dm = game.dm,
               };
               invites.ToList();
            @ViewBag.Invites = invites;

            return View();
        }

        public IActionResult New()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("Home/Index");
            }
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            return View();
        }

        public IActionResult Create(string name)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("Home/Index");
            }
            int user_id = HttpContext.Session.GetInt32("userID") ?? default(int);

            _context.games.Add(new Game { name = name, dm = user_id });
            _context.SaveChanges();

            var game = _context.games.SingleOrDefault(x => x.name == name);

            return Redirect($"View/{game.id}");
        }

        public IActionResult View(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("Home/Index");
            }
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");

            var game = _context.games.SingleOrDefault(x => x.id == id);

            ViewBag.Game = game;
            return View();
        }

        public IActionResult Invite(int id, string username)
        {
            var inviteduser = _context.users.SingleOrDefault(x => x.username == username);

            _context.gamesusers.Add(new GameUser { gameid = id, userid = inviteduser.id });
            _context.SaveChanges();

            return Redirect($"View/{id}");
        }
    }
}