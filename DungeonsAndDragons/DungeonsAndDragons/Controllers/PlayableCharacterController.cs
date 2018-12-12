using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _context.playablecharacters.Add(new PlayableCharacter { name = name });
            _context.SaveChanges();
            return Redirect("New");
        }
    }
}
