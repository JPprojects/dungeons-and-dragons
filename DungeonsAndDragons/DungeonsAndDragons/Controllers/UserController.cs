using DungeonsAndDragons.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StaticHttpContextAccessor.Helpers;

namespace DungeonsAndDragons.Controllers
{
    public class UserController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;
        private readonly SessionHandler _sessionHandler;

        public UserController(DungeonsAndDragonsContext context, SessionHandler sessionHandler)
        {
            _context = context;
            _sessionHandler = sessionHandler;
        }



        public IActionResult Index()
        {
            ViewBag.Username = _sessionHandler.GetSignedInUsername();
            ViewBag.Message = TempData["FlashMessage"];
            return View();
        }



        public IActionResult SignIn(string username, string password)
        {
            User user = Models.User.GetUserByUserName(_context, username);

            if (user != null && Models.User.AuthenticateSignIn(user.password, password))
            {
                _sessionHandler.SetUserSession(user.username, user.id);
                return Redirect("../Game");
            }

            TempData["FlashMessage"] = "Login credentials do not match.";
            return Redirect("/User");

        }



        public IActionResult New()
        {
            ViewBag.Message = TempData["FlashMessage"];
            return View();
        }



        public IActionResult Create(string username, string password)
        {
            var user = Models.User.GetUserByUserName(_context, username);

            if (user != null)
            {
                TempData["FlashMessage"] = "Username already in use";
                return Redirect("New");
            }

            string encryptedPassword = Encryption.EncryptPassword(password);
            User newUser = Models.User.CreateNewUser(_context, username, encryptedPassword);
            _sessionHandler.SetUserSession(newUser.username, newUser.id);

            return Redirect("../Game");

        }



        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
