using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class Game
    {
        public long id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string name { get; set; }
        public User dm_ { get; set; }
    }
}
