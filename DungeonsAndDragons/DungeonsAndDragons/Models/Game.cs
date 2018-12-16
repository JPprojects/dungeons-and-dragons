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



        public static List<Mapping> GetPlayerGames(IQueryable userAcceptedAndPendingGames)
        {
            List<Mapping> acceptedGames = new List<Mapping>();

            foreach (Mapping result in userAcceptedAndPendingGames)
            {
                if (result.playablecharacterid != null)
                {
                    acceptedGames.Add(result);
                }
            }

            return acceptedGames;
        }



        public static List<Mapping> GetInvites(IQueryable userAcceptedAndPendingGames)
        {
            List<Mapping> pendingGames = new List<Mapping>();

            foreach (Mapping result in userAcceptedAndPendingGames)
            {
                if (result.playablecharacterid == null)
                {
                    pendingGames.Add(result);
                }
            }

            return pendingGames;
        }
    }
}
