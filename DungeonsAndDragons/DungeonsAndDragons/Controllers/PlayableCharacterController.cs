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

        public IActionResult Create(string name, int gamesusersid)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("/");
            }

            int userID = HttpContext.Session.GetInt32("userID") ?? default(int);

            _context.playablecharacters.Add(new PlayableCharacter { userid = userID, name = name });
            _context.SaveChanges();

            var result = _context.gamesusers.Find(gamesusersid);
            result.playablecharacterid = 1;

            return Redirect("New");
        }
    }
}