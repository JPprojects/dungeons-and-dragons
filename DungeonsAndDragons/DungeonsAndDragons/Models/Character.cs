using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class Character
    {
        public int id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string name { get; set; }
        public int species_id { get; set; }
    }
}
