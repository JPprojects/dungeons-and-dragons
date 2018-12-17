using System;
using System.Collections.Generic;

namespace DungeonsAndDragons.Models
{
    public class Battle
    {
        public int gameId { get; set; }         public int dmId { get; set; }         public int currentPlayerId { get; set; }         public List<PlayableCharacter> players { get; set; }         public NonPlayableCharacter NPC { get; set; }
    }
}
