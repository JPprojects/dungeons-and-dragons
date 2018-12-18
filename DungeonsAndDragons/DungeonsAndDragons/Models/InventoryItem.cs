using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class InventoryItem
    {
        public int id { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }
        public string imagePath { get; set; }
        public int healingFactor { get; set; }
        public int attack { get; set; }
        public int monetaryValue { get; set; }

    }
}
