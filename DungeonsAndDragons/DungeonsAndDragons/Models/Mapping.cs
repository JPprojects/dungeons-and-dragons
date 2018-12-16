using System;
using System.Linq;

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
        public int playablecharacterhp { get; set; }
        public int playablecharacterattack { get; set; }
        public int characteruserid { get; set; }
        public int speciesid { get; set; }
        public string speciestype { get; set; }
        public string speciesimage { get; set; }
        public int speciesbasehp { get; set; }
        public int speciesbaseattack { get; set; }
        public int? nonplayablecharacterid { get; set; }
        public string nonplayablecharactername { get; set; }
        public int nonplayablecharacterhp { get; set; }
        public int nonplayablecharacterattack { get; set; }


        public static IQueryable GameAndUserJoin(DungeonsAndDragonsContext _context, int loggedinuserid)
        {
            IQueryable useracceptedandpendinggames =
               from gameuser in _context.gamesusers
               join game in _context.games
               on gameuser.gameid equals game.id
               join user in _context.users
               on game.dm equals user.id
               where gameuser.userid == loggedinuserid
               select new Mapping
               {
                   id = gameuser.id,
                   gameid = game.id,
                   gamename = game.name,
                   gamedm = game.dm,
                   playablecharacterid = gameuser.playablecharacterid,
                   userid = user.id,
                   userusername = user.username
               };

            return useracceptedandpendinggames;
        }

      

        public static IQueryable GameUserAndPlayableCharacterJoin(DungeonsAndDragonsContext _context, int gameId)
        {
            IQueryable gameLobbyAcceptedAndPendingPlayers =
               from gameuser in _context.gamesusers
               join user in _context.users
               on gameuser.userid equals user.id
               join character in _context.playablecharacters
               on gameuser.playablecharacterid equals character.id into leftjoin
               from character in leftjoin.DefaultIfEmpty()
               where gameuser.gameid == gameId
               select new Mapping
               {
                   userid = user.id,
                   userusername = user.username,
                   gameid = gameuser.gameid,
                   playablecharacterid = gameuser.playablecharacterid,
                   playablecharactername = character.name
               };

            return gameLobbyAcceptedAndPendingPlayers;
        }



        public static IQueryable SpeciesAndCharacterJoin(DungeonsAndDragonsContext _context)
        {
            return
              from species in _context.species
              join character in _context.playablecharacters
              on species.id equals character.species_id
              select new Mapping
              {
                  speciesid = species.id,
                  speciestype = species.species_type,
                  speciesimage = species.image_path,
                  speciesbasehp = species.base_hp,
                  speciesbaseattack = species.base_attack,
                  playablecharacterid = character.id,
                  playablecharactername = character.name,
                  playablecharacterhp = character.hp,
                  playablecharacterattack = character.attack
              };
        }



        public static IQueryable SpeciesAndNpcJoin(DungeonsAndDragonsContext _context)
        {
            return
              from species in _context.species
              join character in _context.nonplayablecharacters
              on species.id equals character.species_id
              select new Mapping
              {
                  speciesid = species.id,
                  speciestype = species.species_type,
                  speciesimage = species.image_path,
                  speciesbasehp = species.base_hp,
                  speciesbaseattack = species.base_attack,
                  nonplayablecharacterid = character.id,
                  nonplayablecharactername = character.name,
                  nonplayablecharacterhp = character.hp,
                  nonplayablecharacterattack = character.attack
              };
        }
    }
}
