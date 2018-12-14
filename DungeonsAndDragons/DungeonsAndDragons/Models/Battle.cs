using System;
using System.Collections.Generic;

namespace DungeonsAndDragons.Models
{
    public class Battle
    {
        public List<int> players { get; set; }
        public int npc { get; set; }
        public int dm { get; set; }
        public int gameid { get; set; }
    }
}
