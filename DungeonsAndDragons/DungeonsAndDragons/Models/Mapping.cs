using System;
namespace DungeonsAndDragons.Models
{
    public class Mapping
    {
        public int id { get; set; }
        public string gamename { get; set; }
        public int gamedm { get; set; }
        public int gameid { get; set; }
        public int userid { get; set; }
        public string userusername { get; set; }
        public int? playablecharacterid { get; set; }
        public string playablecharactername { get; set; }
        public int characteruserid { get; set; }
        public int speciesid { get; set; }
        public string speciestype { get; set; }
        public string speciesimage { get; set; }
        public int speciesbasehp { get; set; }
        public int speciesbaseattack { get; set; }

    }
}
