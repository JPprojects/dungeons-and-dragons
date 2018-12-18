using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DungeonsAndDragons.Models
{
    public class DungeonsAndDragonsContext : DbContext
    {
        public DungeonsAndDragonsContext(DbContextOptions<DungeonsAndDragonsContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Game> games { get; set; }
        public DbSet<GameUser> gamesusers { get; set; }
        public DbSet<PlayableCharacter> playablecharacters { get; set; }
        public DbSet<NonPlayableCharacter> nonplayablecharacters { get; set; }
        public DbSet<Species> species { get; set; }
        public DbSet<InventoryItem> inventoryitems { get; set; }
        public DbSet<Inventory> inventory { get; set; }
    }
}
