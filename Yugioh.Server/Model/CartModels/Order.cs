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
        public string? DiscountPercent { get; set; }
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

        public void SetTotalPrice(double shipping, double discountPercent)
        {
            if (Cart == null)
            {
                return;
            }
            if (shipping == 0 && discountPercent == 0)
            {
                ShippingPrice = "0";
                TotalPrice = Cart.GetTotalPrice().ToString();
            }
            else if (shipping == 0 && discountPercent != 0)
            {
                ShippingPrice = "0";
                TotalPrice = (Cart.GetTotalPrice() * (1 - discountPercent / 100)).ToString();
            }
            else if (shipping != 0 && discountPercent == 0)
            {
                ShippingPrice = shipping.ToString();
                TotalPrice = (Cart.GetTotalPrice() + shipping).ToString();
            }
            else
            {
                ShippingPrice = shipping.ToString();
                TotalPrice = (Cart.GetTotalPrice() * (1 - discountPercent / 100) + shipping).ToString();
            }
            UpdateTimestamp();
        }
    }
}
