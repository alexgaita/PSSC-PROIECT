
using Domain.Models;
using static Domain.Models.PlacedOrder;
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

        public async Task<Order> AddOrder(CalculatedOrder order)
        {
            var orderDto = new OrderDto()
            {
                Address = order.Address,
                Email = order.Email,
                Total = order.Total
            };
            
            _dbContext.Orders.Add(orderDto);
            await _dbContext.SaveChangesAsync();

            return new Order
            {
                Address = orderDto.Address,
                Email = orderDto.Email,
                Id = orderDto.Id,
                Total = orderDto.Total
            };
        }

        public async Task MapOrderToProducts(int orderId, IReadOnlyCollection<ValidatedOrderProduct> products)
        {
            var orderProducts = products.Select(product => new OrderProductsDto()
            {
                OrderId = orderId,
                ProductId = product.Product.Id,
                Quantity = product.Quantity,
                Price = product.Product.Price
            }).ToList();
            
            _dbContext.OrderProducts.AddRange(orderProducts);
            await _dbContext.SaveChangesAsync();
        }

    }
}
