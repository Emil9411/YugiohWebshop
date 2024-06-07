using Microsoft.EntityFrameworkCore;
using Yugioh.Server.Context;
using Yugioh.Server.Model.CartModels;

namespace Yugioh.Server.Services.CartOrderRepository
{
    public class CartOrderRepo : ICartRepo, IOrderRepo
    {
        private readonly CardsContext _context;
        private readonly ILogger<CartOrderRepo> _logger;

        public CartOrderRepo(CardsContext context, ILogger<CartOrderRepo> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Cart> CreateCart(string userId)
        {
            var cart = new Cart
            {
                UserId = userId
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Cart created for user {userId}");
            return cart;
        }

        public async Task<Cart> GetCart(string userId)
        {
            _logger.LogInformation($"Getting cart for user {userId}");
            return await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(item => item.MonsterProduct)
                .Include(cart => cart.CartItems)
                .ThenInclude(item => item.SpellAndTrapProduct)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);
        }

        public async Task AddCardToCart(string userId, int cardId, int quantity)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}, creating a new cart");
                cart = await CreateCart(userId);
            }
            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == cardId);
            if (cartItem == null)
            {
                _logger.LogInformation($"Adding card {cardId} to cart for user {userId}");
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = cardId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                _logger.LogInformation($"Increasing quantity of card {cardId} in cart for user {userId}");
                cartItem.Quantity += quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCardFromCart(string userId, int cardId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return;
            }
            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == cardId);
            if (cartItem == null)
            {
                _logger.LogInformation($"Card {cardId} not found in cart for user {userId}");
                return;
            }
            _context.CartItems.Remove(cartItem);
            _logger.LogInformation($"Card {cardId} removed from cart for user {userId}");
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCardQuantity(string userId, int cardId, int quantity)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return;
            }
            var cartItem = cart.CartItems.FirstOrDefault(item => item.ProductId == cardId);
            if (cartItem == null)
            {
                _logger.LogInformation($"Card {cardId} not found in cart for user {userId}");
                return;
            }
            cartItem.Quantity = quantity;
            _logger.LogInformation($"Quantity of card {cardId} updated in cart for user {userId}");
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItems(string userId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return new List<CartItem>();
            }
            _logger.LogInformation($"Getting cart items for user {userId}");
            return cart.CartItems.ToList();
        }

        public async Task ClearCart(string userId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return;
            }
            cart.ClearItems();
            _logger.LogInformation($"Cart cleared for user {userId}");
            await _context.SaveChangesAsync();
        }

        public async Task<Order> CheckoutCart(string userId, Order order)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return null;
            }
            order.CartId = cart.CartId;
            order.TotalPrice = cart.GetTotalPrice().ToString();
            order.CreatedAt = DateTime.Now;
            _context.Orders.Add(order);
            cart.IsCheckedOut = true;
            _logger.LogInformation($"Cart checked out for user {userId}");
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteCarts(string userId)
        {
            var carts = await _context.Carts.Where(cart => cart.UserId == userId).ToListAsync();
            if (carts == null)
            {
                _logger.LogInformation($"No carts found for user {userId}");
                return;
            }
            foreach (var cart in carts)
            {
                _context.Carts.Remove(cart);
            }
            _logger.LogInformation($"Carts deleted for user {userId}");
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrder(int orderId)
        {
            _logger.LogInformation($"Getting order {orderId}");
            return await _context.Orders
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.MonsterProduct)
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.SpellAndTrapProduct)
                .FirstOrDefaultAsync(order => order.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrders(string userId)
        {
            _logger.LogInformation($"Getting orders for user {userId}");
            var orderList = await _context.Orders
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.MonsterProduct)
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.SpellAndTrapProduct)
                .Where(order => order.Cart.UserId == userId)
                .ToListAsync();
            if (orderList == null)
            {
                _logger.LogInformation($"No orders found for user {userId}, creating a new cart");
                return new List<Order>();
            }
            return orderList;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            _logger.LogInformation("Getting all orders");
            return await _context.Orders
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.MonsterProduct)
                .Include(order => order.Cart)
                .ThenInclude(cart => cart.CartItems)
                .ThenInclude(item => item.SpellAndTrapProduct)
                .ToListAsync();
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order {order.OrderId} updated");
            return order;
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await GetOrder(orderId);
            if (order == null)
            {
                _logger.LogInformation($"Order {orderId} not found");
                return;
            }
            _context.Orders.Remove(order);
            _logger.LogInformation($"Order {orderId} deleted");
            await _context.SaveChangesAsync();
        }

        public async Task<Order> CreateOrder(string userId, Order order)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return null;
            }
            order.CartId = cart.CartId;
            order.TotalPrice = cart.GetTotalPrice().ToString();
            order.CreatedAt = DateTime.Now;
            _context.Orders.Add(order);
            _logger.LogInformation($"Order created for user {userId}");
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
