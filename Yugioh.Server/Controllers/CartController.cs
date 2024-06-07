using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Yugioh.Server.Model.CartModels;
using Yugioh.Server.Services.CartOrderRepository;

namespace Yugioh.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepo _cartRepo;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartRepo cartRepo, ILogger<CartController> logger)
        {
            _cartRepo = cartRepo;
            _logger = logger;
        }

        [HttpPost("createcart"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<Cart>> CreateCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: CreateCart: Unauthorized access to create cart");
                return Unauthorized("CartController: CreateCart: Unauthorized access to create cart");
            }

            // Check if a cart already exists for the user
            var existingCart = await _cartRepo.GetCart(userId);
            if (existingCart != null)
            {
                return Ok(existingCart);
            }

            var cart = await _cartRepo.CreateCart(userId);
            if (cart == null)
            {
                _logger.LogError("CartController: CreateCart: Failed to create cart");
                return BadRequest("CartController: CreateCart: Failed to create cart");
            }
            return Ok(cart);
        }


        [HttpGet("getcart"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<Cart>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: GetCart: Unauthorized access to get cart");
                return Unauthorized("CartController: GetCart: Unauthorized access to get cart");
            }
            var cart = await _cartRepo.GetCart(userId);
            if (cart == null)
            {
                _logger.LogError("CartController: GetCart: Cart not found");
                return NotFound("CartController: GetCart: Cart not found");
            }
            return Ok(cart);
        }

        [HttpPost("addcard/{cardId}/{quantity}"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<CartItem>> AddCardToCart(int cardId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: AddCardToCart: Unauthorized access to add card to cart");
                return Unauthorized("CartController: AddCardToCart: Unauthorized access to add card to cart");
            }
            await _cartRepo.AddCardToCart(userId, cardId, quantity);
            _logger.LogInformation($"CartController: AddCardToCart: Card {cardId} added to cart for user {userId}");
            return Ok();
        }

        [HttpDelete("removecard/{cardId}"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<CartItem>> RemoveCardFromCart(int cardId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: RemoveCardFromCart: Unauthorized access to remove card from cart");
                return Unauthorized("CartController: RemoveCardFromCart: Unauthorized access to remove card from cart");
            }
            await _cartRepo.RemoveCardFromCart(userId, cardId);
            _logger.LogInformation($"CartController: RemoveCardFromCart: Card {cardId} removed from cart for user {userId}");
            return Ok();
        }

        [HttpPatch("updatecard/{cardId}/{quantity}"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<CartItem>> UpdateCardQuantity(int cardId, int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: UpdateCardQuantity: Unauthorized access to update card quantity in cart");
                return Unauthorized("CartController: UpdateCardQuantity: Unauthorized access to update card quantity in cart");
            }
            await _cartRepo.UpdateCardQuantity(userId, cardId, quantity);
            _logger.LogInformation($"CartController: UpdateCardQuantity: Quantity of card {cardId} updated in cart for user {userId}");
            return Ok();
        }

        [HttpGet("getcartitems"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<List<CartItem>>> GetCartItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: GetCartItems: Unauthorized access to get cart items");
                return Unauthorized("CartController: GetCartItems: Unauthorized access to get cart items");
            }
            var cartItems = await _cartRepo.GetCartItems(userId);
            if (cartItems == null)
            {
                _logger.LogError("CartController: GetCartItems: Cart items not found");
                return NotFound("CartController: GetCartItems: Cart items not found");
            }
            return Ok(cartItems);
        }

        [HttpDelete("clearcart"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<Cart>> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: ClearCart: Unauthorized access to clear cart");
                return Unauthorized("CartController: ClearCart: Unauthorized access to clear cart");
            }
            await _cartRepo.ClearCart(userId);
            _logger.LogInformation($"CartController: ClearCart: Cart cleared for user {userId}");
            return Ok();
        }

        [HttpPost("checkoutcart"), Authorize(Roles = "User, Admin")]
        public async Task<ActionResult<Order>> CheckoutCart([FromBody] Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("CartController: CheckoutCart: Unauthorized access to checkout cart");
                return Unauthorized("CartController: CheckoutCart: Unauthorized access to checkout cart");
            }
            var cart = await _cartRepo.GetCart(userId);
            if (cart == null)
            {
                _logger.LogError("CartController: CheckoutCart: Cart not found");
                return NotFound("CartController: CheckoutCart: Cart not found");
            }
            var orderFromCart = await _cartRepo.CheckoutCart(userId, order);
            if (orderFromCart == null)
            {
                _logger.LogError("CartController: CheckoutCart: Failed to checkout cart");
                return BadRequest("CartController: CheckoutCart: Failed to checkout cart");
            }
            return Ok(orderFromCart);
        }

        [HttpDelete("deletecarts/{userId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCarts(string userId)
        {
            await _cartRepo.DeleteCarts(userId);
            _logger.LogInformation($"CartController: DeleteCarts: Carts deleted for user {userId}");
            return Ok();
        }
    }
}
