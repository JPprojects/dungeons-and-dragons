using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class Game
    {
        public int id { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string name { get; set; }
        public int dm { get; set; }


        public static Game CreateGame(DungeonsAndDragonsContext _context, string gameName, int UserId)
        {
            var game = new Game { name = gameName, dm = UserId };

            _context.games.Add(game);
            _context.SaveChanges();

            return game;
        }


        public static Game getGameById(DungeonsAndDragonsContext _context, int gameId)
        {
            return _context.games.SingleOrDefault(x => x.id == gameId);
        }

        public static IQueryable<Game> GetDMGames(DungeonsAndDragonsContext _context, int loggedinuserid)
        {
            return _context.games.Where(x => x.dm == loggedinuserid);
        }



        public static List<Mapping> GetPlayerGames(IQueryable userAcceptedAndPendingGames)
        {
            List<Mapping> acceptedgames = new List<Mapping>();

            foreach (Mapping result in userAcceptedAndPendingGames)
            {
                if (result.playablecharacterid != null)
                {
                    acceptedgames.Add(result);
                }
            }

            return acceptedgames;
        }



        public static List<Mapping> GetInvites(IQueryable userAcceptedAndPendingGames)
        {
            List<Mapping> pendinggames = new List<Mapping>();

            foreach (Mapping result in userAcceptedAndPendingGames)
            {
                if (result.playablecharacterid == null)
                {
                    pendinggames.Add(result);
                }
            }

            return pendinggames;
        }



        public static bool ValidateAndSendInvite(DungeonsAndDragonsContext _context, int gameId, string inviteeUsername, string signedInUsername)
        {
            User invitedUser = _context.users.SingleOrDefault(x => x.username == inviteeUsername);

            if (ValidateInvite(_context, gameId, invitedUser.username, signedInUsername))
            {
                SendInvite(_context, gameId, invitedUser);
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool ValidateInvite(DungeonsAndDragonsContext _context, int gameId, string inviteeUsername, string signedInUsername)
        {
            var inviteduser = _context.users.SingleOrDefault(x => x.username == inviteeUsername);

            if (inviteduser == null)
            {
                return false;
            }
            else if (inviteeUsername == signedInUsername)
            {
                return false;
            }
            else if (_context.gamesusers.SingleOrDefault(x => x.userid == inviteduser.id & x.gameid == gameId) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public static string GetInvalidInviteString(DungeonsAndDragonsContext _context, int gameId, string inviteeUsername, string signedInUsername)
        {
            var inviteduser = _context.users.SingleOrDefault(x => x.username == inviteeUsername);

            if (inviteduser == null)
            {
                return "Player does not exist.";
            }
            else if (inviteeUsername == signedInUsername)
            {
                return "Cannot invite yourself to a game.";
            }
            else if (_context.gamesusers.SingleOrDefault(x => x.userid == inviteduser.id & x.gameid == gameId) != null)
            {
                return "Player has already been invited.";
            }
            else
            {
                return "Invite failed.";
            }
        }



        public static void SendInvite(DungeonsAndDragonsContext _context, int gameId, User invitedUser)
        {
            _context.gamesusers.Add(new GameUser { gameid = gameId, userid = invitedUser.id });
            _context.SaveChanges();
        }

        public static void DeclineIvite(DungeonsAndDragonsContext _context, int gameUserId)
        {
            GameUser inviteRow = _context.gamesusers.SingleOrDefault(x => x.id == gameUserId);

            _context.gamesusers.Remove(inviteRow);
            _context.SaveChanges();
        }
    }
}
