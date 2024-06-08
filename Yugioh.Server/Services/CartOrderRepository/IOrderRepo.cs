using Yugioh.Server.Model.CartModels;

namespace Yugioh.Server.Services.CartOrderRepository
{
    public interface IOrderRepo
    {
        // Creating a new order for a user after checking out
        Task<Order> CreateOrder(string userId);
        // Getting all the orders of a user
        Task<List<Order>> GetOrders(string userId);
        // Getting an order by order id
        Task<Order> GetOrder(int orderId);
        // Updating the order
        Task<Order> UpdateOrder(Order order);
        // Deleting the order
        Task DeleteOrder(int orderId);
        // Getting all the orders in the system (admin only)
        Task<List<Order>> GetAllOrders();
    }
}
