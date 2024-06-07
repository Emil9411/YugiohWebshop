using Microsoft.EntityFrameworkCore;
using Yugioh.Server.Model.CardModels;
using Yugioh.Server.Model.CartModels;

namespace Yugioh.Server.Context
{
    public class CardsContext : DbContext
    {
        public CardsContext(DbContextOptions<CardsContext> options) : base(options)
        {
        }

        public DbSet<MonsterCard> MonsterCards { get; set; }
        public DbSet<SpellAndTrapCard> SpellAndTrapCards { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonsterCard>().HasIndex(card => new { card.Name, card.CardId }).IsUnique();
            modelBuilder.Entity<SpellAndTrapCard>().HasIndex(card => new { card.Name, card.CardId }).IsUnique();
            modelBuilder.Entity<Cart>().HasIndex(cart => new { cart.UserId }).IsUnique();
            modelBuilder.Entity<CartItem>().HasIndex(item => new { item.CartId, item.ProductId }).IsUnique();
            modelBuilder.Entity<Order>().HasIndex(order => new { order.CartId, order.UserId }).IsUnique();

            modelBuilder.Entity<Cart>()
                .HasMany(cart => cart.CartItems)
                .WithOne(item => item.Cart)
                .HasForeignKey(item => item.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.MonsterProduct)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.SpellAndTrapProduct)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(order => order.Cart)
                .WithOne(cart => cart.Order)
                .HasForeignKey<Order>(order => order.CartId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
