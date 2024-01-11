using System.Collections.Generic;
using Domain.Models;
using LanguageExt;


namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddOrder(PlacedOrder.CalculatedOrder order);

        Task MapOrderToProducts(int orderId, IReadOnlyCollection<ValidatedOrderProduct> products);
    }
}
