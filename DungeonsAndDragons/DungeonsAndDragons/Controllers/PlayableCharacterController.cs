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

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Create(string name)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("/");
            }

            int userID = HttpContext.Session.GetInt32("userID") ?? default(int);

            var character = new PlayableCharacter() { userid = userID, name = name };

            _context.playablecharacters.Add(character);
            _context.SaveChanges();

            var gamesusersid = 1;

            var result = _context.gamesusers.Find(gamesusersid);
            result.playablecharacterid = character.id;
            _context.SaveChanges();





            //var playablecharacter = new PlayableCharacter()
            //{
            //    userid = userID,
            //    name = name
            //};

            //var abc = _context.playablecharacters.Add(new PlayableCharacter { name = name, userid = userID });
            //_context.SaveChanges();
            //var result = _context.gamesusers.Find(gamesusersid);
            //var b = abc.Properties;
            //result.playablecharacterid =
            //_context.SaveChanges();


            return Redirect("New");
        }
    }
}