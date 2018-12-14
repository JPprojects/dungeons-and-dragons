using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Models;
using DungeonsAndDragons.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DungeonsAndDragons.Controllers
{
    public class NonPlayableCharacterController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;
        private readonly IHubContext<DnDHub> _hubcontext;

        public NonPlayableCharacterController(DungeonsAndDragonsContext context, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _hubcontext = hubcontext;
        }

        public IActionResult New(int gameid, int dmid)
        {
            if (HttpContext.Session.GetInt32("userID") != dmid)
            {
                return Redirect("/");
            }

            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.GameID = gameid;
            ViewBag.Species = _context.species.ToList();

            return View();
        }

        public IActionResult Create(string name, int gameid, int species_id)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("/");
            }

            Species species = _context.species.SingleOrDefault(x => x.id == species_id);

            var character = new NonPlayableCharacter() { gameid = gameid, name = name, species_id = species_id, hp = species.base_hp, attack = species.base_attack };
            _context.nonplayablecharacters.Add(character);
            _context.SaveChanges();

            return Redirect($"../Game/View/{gameid}");
        }
    }
}
