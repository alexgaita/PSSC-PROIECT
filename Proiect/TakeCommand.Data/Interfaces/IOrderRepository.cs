using System.Collections.Generic;
using TakeCommand.Data.Models;

namespace TakeCommand.Data.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void RemoveOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int orderId);
        void PrintFistOrder();
    }
}
