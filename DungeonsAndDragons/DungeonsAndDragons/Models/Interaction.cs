using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class Interaction
    {
        public int gameId { get; set; }         public int dmId { get; set; }         public List<PlayableCharacter> players { get; set; }


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
    }
}
