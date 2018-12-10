using System;
using Microsoft.EntityFrameworkCore;

namespace DungeonsAndDragons.Models
{
    public class DungeonsAndDragonsContext : DbContext
    {
        public DungeonsAndDragonsContext(DbContextOptions<DungeonsAndDragonsContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
