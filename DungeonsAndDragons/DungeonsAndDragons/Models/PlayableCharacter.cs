using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class PlayableCharacter : Character
    {
        public int userid { get; set; }

        public static PlayableCharacter CreateNewCharacter(DungeonsAndDragonsContext _context, int userId, string characterName, int speciesId, int baseHP, int baseAttack, string imagePath, int currentHp)
        {
            PlayableCharacter character = new PlayableCharacter() { userid = userId, name = characterName, species_id = speciesId, maxHp = baseHP, attack = baseAttack, imagePath = imagePath, currentHp = currentHp };
            _context.playablecharacters.Add(character);
            _context.SaveChanges();

            return character;
        }



        public static GameUser GenerateCharacter(DungeonsAndDragonsContext _context, int gamesUsersId, int userId, int speciesId, string characterName)
        {
            Species species = Models.Species.GetSpeciesByID(_context, speciesId);

            PlayableCharacter newCharacter = Models.PlayableCharacter.CreateNewCharacter(_context, userId, characterName, speciesId, species.base_hp, species.base_attack, species.image_path, species.base_hp);

            GameUser AssignedCharacter = Models.GameUser.AssignCharacterToGamePlayer(_context, gamesUsersId, newCharacter.id);

            Inventory Coins = Models.Inventory.addItemToInventory(_context, newCharacter.id, 6, 100);

            Inventory Potion = Models.Inventory.addItemToInventory(_context, newCharacter.id, 1, 1);

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



        public static PlayableCharacter GetPlayableCharacterById(DungeonsAndDragonsContext _context, int playableCharacterId)
        {
            return _context.playablecharacters.SingleOrDefault(p => p.id == playableCharacterId);
        }



        public static void UseHealingItem(DungeonsAndDragonsContext _context, int itemId, int playableCharacterId)
        {
            var healingItem = Inventory.GetInventoryItemById(_context, itemId);
            var playableCharacter = GetPlayableCharacterById(_context, playableCharacterId);
            playableCharacter.currentHp += healingItem.healingFactor;
            if (playableCharacter.currentHp > playableCharacter.maxHp) { playableCharacter.currentHp = playableCharacter.maxHp; };
            _context.SaveChanges();
        }
    }
}
