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

        [HttpPost]
        public void SignIn(string username, string password)
        {
            var user = _context.users.SingleOrDefault(c => c.username == username);

            if (user != null && DungeonsAndDragons.Models.User.AuthenticateSignIn(user.password, password))
            {
                HttpContext.Session.SetInt32("userID", user.id);
                HttpContext.Session.SetString("username", user.username);
                Response.Redirect("../Game");
            }
            else
            {
                TempData["FlashMessage"] = "Login credentials do not match.";
                Response.Redirect("/User");
            }
        }

        // GET: /<controller>/
        public IActionResult New()
        {
            ViewBag.Message = TempData["FlashMessage"];
            return View();
        }

        [HttpPost]
        public void Create(string username, string password)
        {
            var user = _context.users.SingleOrDefault(c => c.username == username);
            if (user != null)
            {
                TempData["FlashMessage"] = "Username already in use";
                Response.Redirect("New");
            }
            else
            {
                var encrypted = DungeonsAndDragons.Models.Encryption.EncryptPassword(password);
                _context.users.Add(new User { username = username, password = encrypted });
                _context.SaveChanges();
                var retrievedUser = _context.users.SingleOrDefault(c => c.username == username);
                HttpContext.Session.SetInt32("userID", retrievedUser.id);
                HttpContext.Session.SetString("username", retrievedUser.username);
                Response.Redirect("../Game");
            }
        }

        public void SignOut()
        {
            HttpContext.Session.Clear();
            Response.Redirect("/");
        }
    }
}
