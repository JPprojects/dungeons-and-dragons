using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using DungeonsAndDragons.Models;
using DungeonsAndDragons.Controllers;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class User
    {
        public int id { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string username { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string password { get; set; }



        public static User GetUserByUserName(DungeonsAndDragonsContext _context, string username)
        {
            return _context.users.SingleOrDefault(c => c.username == username);
        }



        public static bool AuthenticateSignIn(string password, string enteredpassword)
        {
            return Encryption.EncryptPassword(enteredpassword) == password;
        }



        public static User CreateNewUser(DungeonsAndDragonsContext _context, string username, string password)
        {
            _context.users.Add(new User
            {
                username = username,
                password = password
            });
            _context.SaveChanges();

            return _context.users.SingleOrDefault(u => u.username == username);
        }


    }

}
