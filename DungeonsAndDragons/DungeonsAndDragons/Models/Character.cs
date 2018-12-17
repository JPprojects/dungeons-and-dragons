using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class Character
    {
        public int id { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string name { get; set; }
        public int species_id { get; set; }
        public int currentHp { get; set; }
        public int maxHp { get; set; }
        public int attack { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string imagePath { get; set; }
    }
}
