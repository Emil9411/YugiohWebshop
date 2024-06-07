using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Model.CartModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public string? UserId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentResult { get; set; }
        public string? ShippingPrice { get; set; }
        public string? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual Cart? Cart { get; set; }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.Now;
        }

        public void SetCart(Cart cart)
        {
            Cart = cart;
            CartId = cart.CartId;
            UpdateTimestamp();
        }

        public void SetTotalPrice()
        {
            if (Cart == null)
            {
                return;
            }
            TotalPrice = Cart.GetTotalPrice().ToString();
            UpdateTimestamp();
        }
    }
}
