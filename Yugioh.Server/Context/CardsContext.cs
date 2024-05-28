using Microsoft.EntityFrameworkCore;
using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Context
{
    public class CardsContext : DbContext
    {
        public CardsContext(DbContextOptions<CardsContext> options) : base(options)
        {
        }

        public DbSet<MonsterCard> MonsterCards { get; set; }
        public DbSet<SpellAndTrapCard> SpellAndTrapCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonsterCard>().HasIndex(card => new { card.Name, card.CardId }).IsUnique();
            modelBuilder.Entity<SpellAndTrapCard>().HasIndex(card => new { card.Name, card.CardId }).IsUnique();
        }
    }
}
