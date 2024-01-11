using Domain.Models;
using static Domain.Models.PlacedOrder;
using static Domain.Models.OrderProducts;


namespace Domain.Operations;

public static class PlaceOrderOperations
{
    
    public static CalculatedOrder CalculateOrderTotal(NotCalculatedOrder order) => new (order.Products,order.Address,order.Email,order.Products.Sum(p => p.Product.Price * p.Quantity));
    
    public static ValidatedOrderedProducts ValidateProducts(List<Product> products,
        IReadOnlyCollection<UnvalidatedOrderProduct> unvalidatedProducts)
    {
        if(products.Count != unvalidatedProducts.Count)
        {
            throw new Exception("The number of products is not the same");
        }
        
        var validatedProducts = new List<ValidatedOrderProduct>();
        
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
            
            validatedProducts.Add(new ValidatedOrderProduct(product, unvalidatedProduct.Quantity));
        }
        
        return new ValidatedOrderedProducts(validatedProducts);
    }
    
}   