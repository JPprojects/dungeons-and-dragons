using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class Inventory
    {
        public int id { get; set; }
        public int chracterid { get; set; }
        public int inventoryItemId { get; set; }
        public int quantity { get; set; }


        public static List<InventoryMapping> GetPlayersInventoryForDisplay(DungeonsAndDragonsContext _context, int characterId)
        {
            IQueryable join = InventoryMapping.InventoryAndIventoryItemJoin(_context, characterId);

            List<InventoryMapping> userInventory = new List<InventoryMapping>();

            foreach (InventoryMapping item in join)
            {
                userInventory.Add(item);
            }

            return userInventory;
        }

        public static List<Inventory> getPlayersInventory(DungeonsAndDragonsContext _context, int chracterId)
        {
            List<Inventory> playersInventory = new List<Inventory>();

            var inventoryQuery = _context.inventory.Where(i => i.chracterid == chracterId);

            foreach (var item in inventoryQuery)
            {
                playersInventory.Add(item);
            }
            return playersInventory;
        }

        public static Inventory addItemToInventory(DungeonsAndDragonsContext _context, int chracterId, int itemId, int quantity)
        {
            List<Inventory> playersCurrentInventory = getPlayersInventory(_context, chracterId);

            Inventory itemToUpdate = playersCurrentInventory.FirstOrDefault(i => i.inventoryItemId == itemId);
            if (itemToUpdate == null)
            {
                itemToUpdate = new Inventory
                {
                    chracterid = chracterId,
                    inventoryItemId = itemId,
                    quantity = quantity
                };
                _context.inventory.Add(itemToUpdate);

            }
            else
            {
                itemToUpdate.quantity = itemToUpdate.quantity + quantity;
            }
            _context.SaveChanges();
            return itemToUpdate;
        }

        public static Inventory RemoveItemFromInventory(DungeonsAndDragonsContext _context, int chracterId, int itemId, int quantity)
        {
            List<Inventory> playersCurrentInventory = getPlayersInventory(_context, chracterId);

            Inventory itemToUpdate = playersCurrentInventory.FirstOrDefault(i => i.inventoryItemId == itemId);
            if (itemToUpdate != null)
            {
                if (itemToUpdate.quantity <= quantity)
                {
                    itemToUpdate.quantity = 0;
                }
                else
                {
                    itemToUpdate.quantity = itemToUpdate.quantity - quantity;
                }
                _context.SaveChanges();
            }

            return itemToUpdate;
        }



        public static InventoryItem GetInventoryItemById(DungeonsAndDragonsContext _context, int itemId)
        {
            return _context.inventoryitems.Single(i => i.id == itemId);
        }
    }
}
