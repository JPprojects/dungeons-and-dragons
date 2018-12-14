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
using StaticHttpContextAccessor.Helpers;

namespace DungeonsAndDragons.Controllers
{
    public class HomeController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;

        public HomeController(DungeonsAndDragonsContext context, SessionHandler sessionHandler)
        {
            _context = context;
            _sessionHandler = sessionHandler;
        }

        public IActionResult Index()
        {
            if (_sessionHandler.UserIsSignedIn()) {Response.Redirect("../Game");}
            return View();
        }
    }
}
