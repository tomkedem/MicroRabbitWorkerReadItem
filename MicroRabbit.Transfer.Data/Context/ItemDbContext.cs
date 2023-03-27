

using MicroRabbit.Transfer.Domian.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Data.Context
{
    public class ItemDbContext : DbContext
    {
        public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options) 
        {            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasOne(i => i.User)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.UserId);
        }

    }
}
