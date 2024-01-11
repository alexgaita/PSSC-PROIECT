using static Domain.Models.PlacedOrder;


namespace Domain.Operations;

public static class PlaceOrderOperations
{
   
    public static IPlacedOrder CalculateOrderTotal(IPlacedOrder placedOrder) => placedOrder.Match(
      whenUnvalidatedPlacedOrder: unvalidatedOrder => unvalidatedOrder,
      whenInvalidPlacedOrder: invalidOrder => invalidOrder,
      whenNotCalculatedOrder: notCalculatedOrder => CalculateTotal(notCalculatedOrder),
      whenCalculatedOrder: calculatedOrder => calculatedOrder,
      whenPublishedOrder: publishedOrder => publishedOrder
    );
    
    private static IPlacedOrder CalculateTotal(NotCalculatedOrder order) => new CalculatedOrder(order.Products,order.Address,order.Email,order.Products.Sum(p => p.Product.Price * p.Quantity));
    
}   