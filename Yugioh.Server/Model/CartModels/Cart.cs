using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Model.CartModels
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string? UserId { get; set; }
        public bool IsCheckedOut { get; set; }
        public double ShippingPrice { get; set; }
        public double DiscountPercent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual Order? Order { get; set; }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }

        public void AddItem(CartItem item)
        {
            CartItems.Add(item);
            UpdateTimestamp();
        }
        public void RemoveItem(CartItem item)
        {
            CartItems.Remove(item);
            UpdateTimestamp();
        }
        public void ClearItems()
        {
            CartItems.Clear();
            UpdateTimestamp();
        }

        public double GetTotalPrice()
        {
            if (CartItems.Count == 0 || CartItems == null)
            {
                return 0;
            }
            double totalPrice = 0;
            foreach (var item in CartItems)
            {
                totalPrice += ConvertStringToDouble(item.Type == "Monster" ? item.MonsterProduct?.Price : item.SpellAndTrapProduct?.Price) * item.Quantity;
            }
            return totalPrice;
        }

        private double ConvertStringToDouble(string value)
        {
            if (value == null || value == "")
            {
                return 0;
            }
            return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
