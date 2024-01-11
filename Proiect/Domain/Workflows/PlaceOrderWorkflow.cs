using Domain.Commands;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using static Domain.Models.OrderProducts;
using static Domain.Models.PlacedOrder;
using static Domain.WorkflowEvents.PlaceOrderEvent;
using static Domain.Operations.PlaceOrderOperations;

namespace Domain.Workflows;

public class PlaceOrderWorkflow
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEventSender _eventSender;
    
    public PlaceOrderWorkflow(IOrderRepository orderRepository, IProductRepository productRepository, IEventSender eventSender)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _eventSender = eventSender;
    }
    
    public async Task<IPlaceOrderEvent> ExecuteAsync(PlaceOrderCommand command)
    {
        try
        {
            var result = await ExecuteWorkflowAsync(command);
            return result;
        }
        catch (Exception e)
        {
            return new OrderPlacedFailedEvent(e.Message);
        }
    }
    
    private async Task<IPlaceOrderEvent> ExecuteWorkflowAsync(PlaceOrderCommand command)
    {
       UnvalidatedPlacedOrder unvalidatedOrder = new(command.UnvalidatedProducts,command.Address, command.Email);

       var products = await _productRepository.GetAllProductsByIds(unvalidatedOrder.UnvalidatedProducts.Select(p => p.ProductId));
       var validatedProducts = ValidateProducts(products, unvalidatedOrder.UnvalidatedProducts);
       
       var placedOrder = await PlaceOrderAsync(validatedProducts, unvalidatedOrder);
       
       await _eventSender.SendAsync("placedOrders",placedOrder);

       return new OrderPlacedSucceededEvent(placedOrder.Id);
    }

    private async Task<Order> PlaceOrderAsync(ValidatedOrderedProducts products, UnvalidatedPlacedOrder unvalidatedOrder)
    {
        NotCalculatedOrder notCalculatedOrder = new(products.ValidatedProducts, unvalidatedOrder.Address, unvalidatedOrder.Email);
        var calculatedOrder =  CalculateOrderTotal(notCalculatedOrder);
        var savedOrder = await SaveOrder(calculatedOrder);
        return savedOrder;
    }
    
    private async Task<Order> SaveOrder(CalculatedOrder order)
    {
        var savedOrder = await _orderRepository.AddOrder(order);
        await _productRepository.UpdateProductsStock(order.ValidatedProducts);
        await _orderRepository.MapOrderToProducts(savedOrder.Id, order.ValidatedProducts);
        return savedOrder;
    }
    
}