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

namespace DungeonsAndDragons.Controllers
{
    public class UserController : Controller
    {

        private readonly DungeonsAndDragonsContext _context;

        public UserController(DungeonsAndDragonsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("username");
            ViewBag.Message = TempData["FlashMessage"];
            return View();

        }

        public IActionResult SignIn(string username, string password)
        {
            var user = _context.users.SingleOrDefault(c => c.username == username);

            if (user != null && DungeonsAndDragons.Models.User.AuthenticateSignIn(user.password, password))
            {
                HttpContext.Session.SetInt32("userID", user.id);
                HttpContext.Session.SetString("username", user.username);
                return Redirect("../Game");
            }
            else
            {
                TempData["FlashMessage"] = "Login credentials do not match.";
                return Redirect("/User");
            }
        }

        public IActionResult New()
        {
            ViewBag.Message = TempData["FlashMessage"];
            return View();
        }

        public IActionResult Create(string username, string password)
        {
            var user = _context.users.SingleOrDefault(c => c.username == username);
            if (user != null)
            {
                TempData["FlashMessage"] = "Username already in use";
                return Redirect("New");
            }
            else
            {
                var encryptedPassword = DungeonsAndDragons.Models.Encryption.EncryptPassword(password);

                var retrievedUser = Models.User.RegisterNewUser(_context, username, encryptedPassword);


                //_context.users.Add(new User { username = username, password = encryptedPassword });
                //_context.SaveChanges();
                //var retrievedUser = _context.users.SingleOrDefault(c => c.username == username);

                HttpContext.Session.SetInt32("userID", retrievedUser.id);
                HttpContext.Session.SetString("username", retrievedUser.username);
                return Redirect("../Game");
            }
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
