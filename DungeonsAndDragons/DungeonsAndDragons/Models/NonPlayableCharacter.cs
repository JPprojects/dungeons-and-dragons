using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class NonPlayableCharacter : Character
    {
        public int gameid { get; set; }



        public static NonPlayableCharacter CreateNonPlayableCharacter(DungeonsAndDragonsContext _context, int gameid, string chracterName, int speciesId, int hp, int attack)
        {
            var character = new NonPlayableCharacter() { gameid = gameid, name = chracterName, species_id = speciesId, hp = hp, attack = attack };
            _context.nonplayablecharacters.Add(character);
            _context.SaveChanges();

            return character;
        }



        public static NonPlayableCharacter GenerateNPC(DungeonsAndDragonsContext _context, int gameId, int species_id, string characterName)
        {
            Species species = Species.GetSpeciesByID(_context, species_id);

            return CreateNonPlayableCharacter(_context, gameId, characterName, species_id, species.base_hp, species.base_attack);
        }



        public static Mapping GetStatsForNpcCharacter(DungeonsAndDragonsContext _context, int characterId)
        {
            IQueryable SpeciesandNPCJoin = Mapping.SpeciesAndNpcJoin(_context);

            Mapping characterStats = new Mapping();

            foreach (Mapping character in SpeciesandNPCJoin)
            {
                if (character.nonplayablecharacterid == characterId)
                {
                    characterStats = character;
                }
            }
            return characterStats;
        }
    }
}
