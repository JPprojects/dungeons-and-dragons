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
            int dmId = _context.games.SingleOrDefault(x => x.id == gameId).dm;
            NonPlayableCharacter npc = _context.nonplayablecharacters.SingleOrDefault(x => x.id == npcId);
            List<GameUser> gameUserResults = _context.gamesusers.Where(x => x.gameid == gameId & x.playablecharacterid != null).ToList();
            List<PlayableCharacter> players = new List<PlayableCharacter> { };

            foreach (var result in gameUserResults)
            {
                var player = _context.playablecharacters.SingleOrDefault(x => x.id == result.playablecharacterid);
                players.Add(player);
            }

            int currentPlayerId;
            if (players.Count == 0)
            {
                currentPlayerId = 0;
            }
            else
            {
                currentPlayerId = players.First().id;
            }

            var battle = new Battle() { gameId = gameId, dmId = dmId, NPC = npc, players = players, currentPlayerId = currentPlayerId };
            return battle;
        }

        public static bool IsUserInGame(DungeonsAndDragonsContext _context, int loggedInUserId, int gameId)
        {
            GameUser check = _context.gamesusers.SingleOrDefault(x => x.gameid == gameId & x.userid == loggedInUserId);
            var dmcheck = _context.games.Find(gameId);



            if (check != null)
            {
                return true;
            }
            else if (dmcheck.dm == loggedInUserId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void UpdateNpcHp(DungeonsAndDragonsContext _context, int npcId, int newHp)
        {
            var character = _context.nonplayablecharacters.Find(npcId);
            character.currentHp = newHp;
            _context.SaveChanges();
        }

        public static void UpdatePlayerHp(DungeonsAndDragonsContext _context, int characterId, int newHp)
        {
            var character = _context.playablecharacters.Find(characterId);
            character.currentHp = newHp;
            _context.SaveChanges();
        }
    }
}
