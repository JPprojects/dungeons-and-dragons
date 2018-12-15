using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class Species
    {
        public int id { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string species_type { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string image_path { get; set; }
        public int base_hp { get; set; }
        public int base_attack { get; set; }
    }
}
