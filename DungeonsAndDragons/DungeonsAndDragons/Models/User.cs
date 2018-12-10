using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class User
    {
        public long id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string username { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string password { get; set; }
    }
}
