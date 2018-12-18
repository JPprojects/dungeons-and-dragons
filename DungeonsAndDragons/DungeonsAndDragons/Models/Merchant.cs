using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class Merchant : Interaction
    { 
        public static Merchant StartMerchant(DungeonsAndDragonsContext _context, int gameId)
        {
            int dmId = _context.games.SingleOrDefault(x => x.id == gameId).dm;
            List<GameUser> gameUserResults = _context.gamesusers.Where(x => x.gameid == gameId & x.playablecharacterid != null).ToList();
            List<PlayableCharacter> players = new List<PlayableCharacter> { };

            foreach (var result in gameUserResults)
            {
                var player = _context.playablecharacters.SingleOrDefault(x => x.id == result.playablecharacterid);
                players.Add(player);
            }

            var merchant = new Merchant() { gameId = gameId, dmId = dmId, players = players };
            return merchant;
        }

    }
}
