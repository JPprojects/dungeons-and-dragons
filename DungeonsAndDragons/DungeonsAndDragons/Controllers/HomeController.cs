using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            if (HttpContext.Session.GetString("username") != null)
            {
                Response.Redirect("../Game");
            }
            return View();
        }
    }
}
