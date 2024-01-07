
using Domain.Models;
using Domain.Repositories;
using TakeCommand.Data.Models;

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
            OrderDto orderDto = new OrderDto()
            {
                Address = order.Address,
                Email = order.Email,
                Total = order.Total
            };
            _dbContext.Orders.Add(orderDto);
            _dbContext.SaveChanges();
        }

        public void RemoveOrder(Order order)
        {
            OrderDto orderDto = new OrderDto()
            {
                Id = order.Id,
                Address = order.Address,
                Email = order.Email,
                Total = order.Total
            };
            _dbContext.Orders.Remove(orderDto);
            _dbContext.SaveChanges();
        }

        public Order? GetOrderByIdOrNull(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                return new Order()
                {
                    Address = order.Address,
                    Email = order.Email,
                    Total = order.Total,
                    Id = order.Id
                };
            }

            return null;
        }
        
    }
}
