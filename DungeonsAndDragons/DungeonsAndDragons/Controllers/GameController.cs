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
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.id = HttpContext.Session.GetInt32("userID");
            @ViewBag.DMGames = _context.games.Where(x => x.dm == 1);

            // Needs refactoring to use join table.
            var playergames = _context.gamesusers.Where(x => x.userid == 1);
            List<Game> playergameslist = new List<Game>();
            foreach (var game in playergames)
            {
                playergameslist.Add(_context.games.SingleOrDefault(x => x.id == game.gameid));
            }

            @ViewBag.PlayerGames = playergameslist;
            //End refactoring

            @ViewBag.Invites = "test";

            return View();
        }

        public IActionResult New()
        {
            //if (HttpContext.Session.GetString("username") == null)
            //{
            //    return Redirect("Home/Index");
            //}
            return View();
        }

        public IActionResult Create(string name)
        {
            //if (HttpContext.Session.GetString("username") == null)
            //{
            //    return Redirect("Home/Index");
            //}
            _context.games.Add(new Game { name = name });
            _context.SaveChanges();
            return Redirect("Index");
        }

        public IActionResult View(int id)
        {
            return View();
        }
    }
}