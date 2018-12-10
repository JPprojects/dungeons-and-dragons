using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DungeonsAndDragons.Models;

namespace DungeonsAndDragons.Controllers
{
    public class HomeController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;

        public HomeController(DungeonsAndDragonsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Hello = _context.users.ToList();
            return View();
        }
    }
}
