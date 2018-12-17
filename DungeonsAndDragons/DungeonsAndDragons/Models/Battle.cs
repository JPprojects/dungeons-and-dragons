using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class Battle
    {
        public int gameId { get; set; }         public int dmId { get; set; }         public int currentPlayerId { get; set; }         public List<PlayableCharacter> players { get; set; }         public NonPlayableCharacter NPC { get; set; }

        public static Battle StartBattle(DungeonsAndDragonsContext _context, int gameId, int npcId)
        {
            int dmId = _context.games.SingleOrDefault(x => x.id == gameId).id;
            NonPlayableCharacter npc = _context.nonplayablecharacters.SingleOrDefault(x => x.id == npcId);
            List<GameUser> gameUserResults = _context.gamesusers.Where(x => x.gameid == gameId).ToList();
            List<PlayableCharacter> players = new List<PlayableCharacter> { };

            foreach (var result in gameUserResults)
            {
                var player = _context.playablecharacters.SingleOrDefault(x => x.id == result.playablecharacterid);
                players.Add(player);
            }

            var currentPlayerId = players.First().id;

            var battle = new Battle() { gameId = gameId, dmId = dmId, NPC = npc, players = players, currentPlayerId = currentPlayerId };
            return battle;
        }
    }
}
