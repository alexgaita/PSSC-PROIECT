using System.Collections.Generic;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void RemoveOrder(Order order);
        Order? GetOrderByIdOrNull(int orderId);
    }
}
