using System.Collections.Generic;
using Domain.Models;
using LanguageExt;


namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        TryAsync<Order> AddOrder(Order order);
        TryAsync<Unit> RemoveOrder(Order order);
        TryAsync<Order> GetOrderById(int orderId);
    }
}
