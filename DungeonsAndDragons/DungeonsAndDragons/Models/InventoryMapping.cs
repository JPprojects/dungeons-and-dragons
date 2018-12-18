using System;
using System.Linq;

namespace DungeonsAndDragons.Models
{
    public class InventoryMapping
    {
        public int id { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }
        public string imagePath { get; set; }
        public int healingFactor { get; set; }
        public int attack { get; set; }
        public int quantity { get; set; }
        public int monetaryValue { get; set; }



        public static IQueryable InventoryAndIventoryItemJoin(DungeonsAndDragonsContext _context, int charcterId)
        {
            IQueryable UsersInventory =
            from inventory in _context.inventory
            join InventoryItem in _context.inventoryitems
            on inventory.inventoryItemId equals InventoryItem.id
            where inventory.chracterid == charcterId
            select new InventoryMapping
            {
                id = inventory.id,
                itemName = InventoryItem.itemName,
                itemType = InventoryItem.itemType,
                imagePath = InventoryItem.imagePath,
                healingFactor = InventoryItem.healingFactor,
                attack = InventoryItem.attack,
                quantity = inventory.quantity,
                monetaryValue = InventoryItem.monetaryValue
            };

            return UsersInventory;
        }
    }
}
