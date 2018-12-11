using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class Game
    {
        public int id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string name { get; set; }
        public int dm { get; set; }
    }
}
