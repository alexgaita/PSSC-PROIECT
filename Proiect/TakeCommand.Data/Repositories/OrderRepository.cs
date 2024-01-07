using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TakeCommand.Data.Models;
using TakeCommand.Data.Interfaces;
using TakeCommand.Data;

namespace TakeCommand.Data.Repositories
{

    public class OrderRepository : IOrderRepository
    {
        private readonly ShopContext _dbContext;

        public OrderRepository(ShopContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        public void RemoveOrder(Order order)
        {
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _dbContext.Orders.ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
        }

        public void PrintFistOrder()
        {
            var myOrder = _dbContext.Orders.FirstOrDefault(); // Fetch a specific order
            

            if (myOrder != null)
            {
                List<OrderProducts> orderProducts = _dbContext.OrderProducts.Where(op => op.OrderId == myOrder.Id).ToList();

                foreach (var orderProduct in orderProducts)
                {
                    Console.WriteLine(orderProduct.ProductId);
                }
            }
        }
    }
}
