using System.ComponentModel.DataAnnotations;
using Yugioh.Server.Model.CardModels;

namespace Yugioh.Server.Model.CartModels
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public bool HasInInventory { get; set; }
        public string? Type { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual MonsterCard? MonsterProduct { get; set; }
        public virtual SpellAndTrapCard? SpellAndTrapProduct { get; set; }


        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            Quantity -= quantity;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void SetMonsterProduct(MonsterCard monsterCard)
        {
            MonsterProduct = monsterCard;
            ProductId = monsterCard.CardId;
            Type = "Monster";
        }

        public void SetSpellAndTrapProduct(SpellAndTrapCard spellAndTrapCard)
        {
            SpellAndTrapProduct = spellAndTrapCard;
            ProductId = spellAndTrapCard.CardId;
            Type = "SpellAndTrap";
        }

        public void SetCart(Cart cart)
        {
            Cart = cart;
            CartId = cart.CartId;
        }
    }
}
