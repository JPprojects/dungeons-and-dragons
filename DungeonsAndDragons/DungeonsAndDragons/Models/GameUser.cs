using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonsAndDragons.Models
{
    public class GameUser
    {
        public int id { get; set; }
        public int gameid { get; set; }
        public int userid { get; set; }
        public int playablecharacterid { get; set; }
    }
}
