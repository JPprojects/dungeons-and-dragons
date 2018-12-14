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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DungeonsAndDragons.Controllers
{
    public class PlayableCharacterController : Controller
    {
        private readonly DungeonsAndDragonsContext _context;
        private readonly IHubContext<DnDHub> _hubcontext;

        public PlayableCharacterController(DungeonsAndDragonsContext context, IHubContext<DnDHub> hubcontext)
        {
            _context = context;
            _hubcontext = hubcontext;
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

            Species species = _context.species.SingleOrDefault(x => x.id == species_id);

            var character = new PlayableCharacter() { userid = userID, name = name, species_id = species_id, hp = species.base_hp, attack = species.base_attack };
            _context.playablecharacters.Add(character);
            _context.SaveChanges();

            var result = _context.gamesusers.Find(gamesusersid);

            if (gamesusersid != 0)
            {
                result.playablecharacterid = character.id;
                _context.SaveChanges();
            }

            IQueryable games =
               from gameuser in _context.gamesusers
               join user in _context.users
               on gameuser.userid equals user.id
               join playablecharacters in _context.playablecharacters
               on gameuser.playablecharacterid equals playablecharacters.id
               where gameuser.gameid == result.gameid
               select new Mapping
               {
                   userid = user.id,
                   userusername = user.username,
                   playablecharacterid = gameuser.playablecharacterid,
                   playablecharactername = playablecharacters.name
               };

            List<Mapping> acceptedplayers = new List<Mapping>();
            List<Mapping> pendingplayers = new List<Mapping>();

            foreach (Mapping game in games)
            {
                if (game.playablecharacterid != null)
                {
                    acceptedplayers.Add(game);
                }
                else
                {
                    pendingplayers.Add(game);
                }
            }

            _hubcontext.Clients.All.SendAsync("UpdatePlayerInvites", JsonConvert.SerializeObject(acceptedplayers), JsonConvert.SerializeObject(pendingplayers));

            return Redirect($"../Game/View/{result.gameid}");
        }

        public IActionResult View(int id)
        {
            if (HttpContext.Session.GetInt32("userID") == null)
            {
                return Redirect("../../Home");
            }

            ViewBag.Username = HttpContext.Session.GetString("username");

            IQueryable result =
               from species in _context.species
               join character in _context.playablecharacters
               on species.id equals character.species_id
               select new Mapping
               {
                   speciesid = species.id,
                   speciestype = species.species_type,
                   speciesimage = species.image_path,
                   speciesbasehp = species.base_hp,
                   speciesbaseattack = species.base_attack,
                   playablecharacterid = character.id,
                   playablecharactername = character.name,
                   playablecharacterhp = character.hp,
                   playablecharacterattack = character.attack

               };

            foreach (Mapping character in result)
            {
                if (character.playablecharacterid == id)
                {
                    ViewBag.Character = character;
                }
            }

            return View();
        }
    }
}