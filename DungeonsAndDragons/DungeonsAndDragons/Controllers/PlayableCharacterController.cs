using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DungeonsAndDragons.Controllers
{
    public class PlayableCharacterController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;

        public PlayableCharacterController(DungeonsAndDragonsContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult New(int gamesusersid)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("/");
            }

            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.GamesUsersID = gamesusersid;

            ViewBag.Species = _context.species.ToList();

            return View();
        }

        public IActionResult Create(string name, int species_id, int gamesusersid = 0)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("/");
            }

            int userID = HttpContext.Session.GetInt32("userID") ?? default(int);

            var character = new PlayableCharacter() { userid = userID, name = name, species_id = species_id };
            _context.playablecharacters.Add(character);
            _context.SaveChanges();

            var result = _context.gamesusers.Find(gamesusersid);

            if (gamesusersid != 0)
            {
                result.playablecharacterid = character.id;
                _context.SaveChanges();
            }

            return Redirect($"../Game/View/{result.gameid}");
        }
    }
}