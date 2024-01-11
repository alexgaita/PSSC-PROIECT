
using Domain.Models;
using Domain.Repositories;
using LanguageExt;
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

        public TryAsync<Order> AddOrder(Order order) => async () =>
        {
            var orderDto = new OrderDto()
            {
                Address = order.Address,
                Email = order.Email,
                Total = order.Total
            };
            
            _dbContext.Orders.Add(orderDto);
            await _dbContext.SaveChangesAsync();

            return new Order()
            {
                Address = orderDto.Address,
                Email = orderDto.Email,
                Total = orderDto.Total,
                Id = orderDto.Id
            };
        };

        public TryAsync<Unit> RemoveOrder(Order order) => async () =>
        {
            OrderDto orderDto = new OrderDto()
            {
                Id = order.Id,
                Address = order.Address,
                Email = order.Email,
                Total = order.Total
            };
            _dbContext.Orders.Remove(orderDto);
            await _dbContext.SaveChangesAsync();
            return Unit.Default;
        };

        public TryAsync<Order> GetOrderById(int orderId) => async () =>
        {
            var order = await _dbContext.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with id {orderId} was not found");
            }

            return new Order()
            {
                Address = order.Address,
                Email = order.Email,
                Total = order.Total,
                Id = order.Id
            };
        };

    }
}
