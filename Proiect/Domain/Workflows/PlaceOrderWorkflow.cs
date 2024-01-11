using Domain.Commands;
using Domain.Models;
using Domain.Repositories;
using Domain.WorkflowEvents;
using LanguageExt;
using static Domain.Models.OrderProducts;
using static Domain.Models.PlacedOrder;
using static Domain.WorkflowEvents.PlaceOrderEvent;
using static Domain.Operations.PlaceOrderOperations;

using static LanguageExt.Prelude;



namespace Domain.Workflows;

public class PlaceOrderWorkflow
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    
    public PlaceOrderWorkflow(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<IPlaceOrderEvent> ExecuteAsync(PlaceOrderCommand command)
    {
       UnvalidatedPlacedOrder unvalidatedOrder = new(command.UnvalidatedProducts,command.Address, command.Email);

       var result = from products in  _productRepository
               .GetAllProductsByIds(unvalidatedOrder.UnvalidatedProducts.Select(product => product.ProductId))
               .ToEither(ex =>
                   new InvalidPlacedOrder("Something went wrong while trying to fetch products") as IPlacedOrder)
           let validatedProducts = ValidateProducts(products, unvalidatedOrder.UnvalidatedProducts.ToList())
           // from publishedOrder in ExecuteWorkflowAsync(validatedProducts , unvalidatedOrder)
           select products;
           

        return new OrderPlacedSucceededEvent(1);
        
    }

    private ValidatedOrderedProducts ValidateProducts(List<Product> products,
        List<UnvalidatedOrderProduct> unvalidatedProducts)
    {
        
        if(products.Count != unvalidatedProducts.Count)
        {
            throw new Exception("The number of products is not the same");
        }
        
        var valitatedProducts = new List<ValidatedOrderProduct>();
        
        foreach (var product in products)
        {
            var unvalidatedProduct = unvalidatedProducts.FirstOrDefault(p => p.ProductId == product.Id);
            if (unvalidatedProduct == null)
            {
                throw new Exception("The product does not exist in the database");
            }

            if (product.Stock < unvalidatedProduct.Quantity)
            {
                throw new Exception("The stock is not enough");
            }
            
            valitatedProducts.Add(new ValidatedOrderProduct(product, unvalidatedProduct.Quantity));
        }
        
        return new ValidatedOrderedProducts(valitatedProducts);
    }

    private async Task<Either<IPlacedOrder, PublishedOrder>> ExecuteWorkflowAsync(ValidatedOrderedProducts products, UnvalidatedPlacedOrder unvalidatedOrder)
    {
        NotCalculatedOrder notCalculatedOrder = new(products.ValidatedProducts, unvalidatedOrder.Address, unvalidatedOrder.Email);
        var calculatedOrder =  CalculateOrderTotal(notCalculatedOrder);
        var savedOrder = await SaveOrderToDb(calculatedOrder);
        
        return calculatedOrder.Match<Either<IPlacedOrder, PublishedOrder>>(
            whenUnvalidatedPlacedOrder: unvalidatedOrder => Left(unvalidatedOrder as IPlacedOrder),
            whenInvalidPlacedOrder: invalidOrder => Left(invalidOrder as IPlacedOrder),
            whenNotCalculatedOrder: notCalculatedOrder => Left(notCalculatedOrder as IPlacedOrder),
            whenCalculatedOrder: calculatedOrder => Left(calculatedOrder as IPlacedOrder),
            whenPublishedOrder: publishedOrder => Right(publishedOrder as PublishedOrder)
        );
    }
    
    private async Task<IPlacedOrder> SaveOrder(CalculatedOrder order)
    {
        var savedOrder = await _orderRepository.AddOrder(new Order{Address = order.Address, Email = order.Email, Total = order.Total}).ToEither(ex=> new InvalidPlacedOrder("Something went wrong while saving the order") as IPlacedOrder);
        // await _productRepository.updateProductsStock(order.ValidatedProducts.Select(p => p.Product).ToList());
        return order;
        // return new CalculatedOrder(savedOrder.ValidatedProducts, savedOrder.Address, savedOrder.Email, savedOrder.Total, savedOrder.Id);
    }
    
    private  Task<IPlacedOrder> SaveOrderToDb(IPlacedOrder placedOrder) => placedOrder.MatchAsync(
        whenUnvalidatedPlacedOrder: async unvalidatedOrder => unvalidatedOrder,
        whenInvalidPlacedOrder: async invalidOrder => invalidOrder,
        whenNotCalculatedOrder: async notCalculatedOrder => notCalculatedOrder,
        whenCalculatedOrder:  calculatedOrder => SaveOrder(calculatedOrder),
        whenPublishedOrder: async publishedOrder => publishedOrder
    );







}