using Yugioh.Server.Model.CartModels;

namespace Yugioh.Server.Services.CartOrderRepository
{
    public interface ICartRepo
    {
        // Creating a new cart for a user
        Task<Cart> CreateCart(string userId);
        // Getting a cart by user id
        Task<Cart> GetCart(string userId);
        // Adding a card to the cart
        Task AddCardToCart(string userId, int cardId, int quantity);
        // Removing a card from the cart
        Task RemoveCardFromCart(string userId, int cardId);
        // Updating the quantity of a card in the cart
        Task UpdateCardQuantity(string userId, int cardId, int quantity);
        // Getting all the items in the cart
        Task<List<CartItem>> GetCartItems(string userId);
        // Clearing the cart
        Task ClearCart(string userId);
        // Checking out the cart
        Task<Cart> CheckoutCart(string userId);
        // Delete the carts of a user (admin only)
        Task DeleteCarts(string userId);
        Task<Cart> UpdateCart(string userId, Cart cart);
    }
}
