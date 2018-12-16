using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class PlayableCharacter : Character
    {
        public int userid { get; set; }

        public static PlayableCharacter CreateNewCharacter(DungeonsAndDragonsContext _context, int userId, string characterName, int speciesId, int baseHP, int baseAttack)
        {
            PlayableCharacter character = new PlayableCharacter() { userid = userId, name = characterName, species_id = speciesId, hp = baseHP, attack = baseAttack };
            _context.playablecharacters.Add(character);
            _context.SaveChanges();

            return character;
        }



        public static GameUser GenerateCharacter(DungeonsAndDragonsContext _context, int gamesUsersId, int userId, int speciesId, string characterName)
        {
            Species species = Models.Species.GetSpeciesByID(_context, speciesId);

            PlayableCharacter newCharacter = Models.PlayableCharacter.CreateNewCharacter(_context, userId, characterName, speciesId, species.base_hp, species.base_attack);

            GameUser AssignedCharacter = Models.GameUser.AssignCharacterToGamePlayer(_context, gamesUsersId, newCharacter.id);

            return AssignedCharacter;
        }



        public static Mapping GetStatsForUserGeneratedCharacter(DungeonsAndDragonsContext _context, int characterId)
        {
            IQueryable speciesAndCharacterJoin = Mapping.SpeciesAndCharacterJoin(_context);

            Mapping characterStats = new Mapping();

            foreach (Mapping character in speciesAndCharacterJoin)
            {
                if (character.playablecharacterid == characterId)
                {
                    characterStats = character;
                }
            }
            return characterStats;
        }

    }
}
