﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Cart> CheckoutCart(string userId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return null;
            }
            foreach (var item in cart.CartItems)
            {
                if (item.MonsterProduct != null)
                {
                    if (item.Quantity > item.MonsterProduct.Inventory)
                    {
                        _logger.LogInformation($"Not enough quantity of card {item.ProductId} in inventory");
                        item.HasInInventory = false;
                        return null;
                    }
                    MonsterRemoveFromInventory(item.ProductId, item.Quantity);
                }
                else if (item.SpellAndTrapProduct != null)
                {
                    if (item.Quantity > item.SpellAndTrapProduct.Inventory)
                    {
                        _logger.LogInformation($"Not enough quantity of card {item.ProductId} in inventory");
                        item.HasInInventory = false;
                        return null;
                    }
                    SpellAndTrapRemoveFromInventory(item.ProductId, item.Quantity);
                }
                item.HasInInventory = true;
            }
            var cartItemsOk = cart.CartItems.All(item => item.HasInInventory);
            if (!cartItemsOk)
            {
                _logger.LogInformation($"Not enough quantity of cards in inventory");
                return null;
            }
            _logger.LogInformation($"Cart checked out for user {userId}");
            cart.IsCheckedOut = true;
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> UpdateCart(string userId, Cart cart)
        {
            var userCart = await GetCart(userId);
            if (userCart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return null;
            }
            userCart.ShippingPrice = cart.ShippingPrice;
            userCart.DiscountPercent = cart.DiscountPercent;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Cart updated for user {userId}");
            return userCart;
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

        public async Task<Order> CreateOrder(string userId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart not found for user {userId}");
                return null;
            }
            if (!cart.IsCheckedOut)
            {
                _logger.LogInformation($"Cart not checked out for user {userId}");
                return null;
            }
            var order = new Order
            {
                UserId = userId,
                ShippingPrice = cart.ShippingPrice.ToString(),
                DiscountPercent = cart.DiscountPercent.ToString()
            };
            order.SetCart(cart);
            order.SetTotalPrice(cart.ShippingPrice, cart.DiscountPercent);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order created for user {userId}");
            return order;
        }

        private async void MonsterAddToInventory(int cardId, int quantity)
        {
            var monsterCard = _context.MonsterCards.FirstOrDefault(card => card.CardId == cardId);
            if (monsterCard == null)
            {
                _logger.LogInformation($"Monster card {cardId} not found");
                return;
            }
            monsterCard.Inventory += quantity;
            _logger.LogInformation($"Inventory of monster card {cardId} updated");
            await _context.SaveChangesAsync();
        }

        private async void SpellAndTrapAddToInventory(int cardId, int quantity)
        {
            var spellAndTrapCard = _context.SpellAndTrapCards.FirstOrDefault(card => card.CardId == cardId);
            if (spellAndTrapCard == null)
            {
                _logger.LogInformation($"Spell and trap card {cardId} not found");
                return;
            }
            spellAndTrapCard.Inventory += quantity;
            _logger.LogInformation($"Quantity of spell and trap card {cardId} updated");
            await _context.SaveChangesAsync();
        }

        private async void MonsterRemoveFromInventory(int cardId, int quantity)
        {
            var monsterCard = _context.MonsterCards.FirstOrDefault(card => card.CardId == cardId);
            if (monsterCard == null)
            {
                _logger.LogInformation($"Monster card {cardId} not found");
                return;
            }
            monsterCard.Inventory -= quantity;
            _logger.LogInformation($"Inventory of monster card {cardId} updated");
            await _context.SaveChangesAsync();
        }

        private async void SpellAndTrapRemoveFromInventory(int cardId, int quantity)
        {
            var spellAndTrapCard = _context.SpellAndTrapCards.FirstOrDefault(card => card.CardId == cardId);
            if (spellAndTrapCard == null)
            {
                _logger.LogInformation($"Spell and trap card {cardId} not found");
                return;
            }
            spellAndTrapCard.Inventory -= quantity;
            _logger.LogInformation($"Quantity of spell and trap card {cardId} updated");
            await _context.SaveChangesAsync();
        }
    }
}
