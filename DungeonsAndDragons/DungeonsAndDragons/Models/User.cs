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
using System.Reflection.Metadata.Ecma335;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class User
    {
        public int id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string username { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string password { get; set; }

        public static bool AuthenticateSignIn(string password, string enteredpassword)
        {

            return Encryption.EncryptPassword(enteredpassword) == password;
        }


        public static User RegisterNewUser(DungeonsAndDragonsContext dbcontext, string username, string password)
        {
            dbcontext.users.Add(new User
            {
                username = username,
                password = password
            });
            dbcontext.SaveChanges();

            return dbcontext.users.SingleOrDefault(c => c.username == username);
        }


    }

}
