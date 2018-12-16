using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class GameUser
    {
        public int id { get; set; }
        public int gameid { get; set; }
        public int userid { get; set; }
        public int? playablecharacterid { get; set; }



        public static GameUser GetGameUserByID(DungeonsAndDragonsContext _context, int gameUserId)
        {
            return _context.gamesusers.Find(gameUserId);
        }



        public static GameUser AssignCharacterToGamePlayer(DungeonsAndDragonsContext _context, int gameUserId, int characterId)
        {
            var gamePlayer = GetGameUserByID(_context, gameUserId);

            if (gameUserId != 0)
            {
                gamePlayer.playablecharacterid = characterId;
                _context.SaveChanges();
            }

            return gamePlayer;
        }

    }
}
