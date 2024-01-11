using CSharp.Choices;

namespace Domain.Models;

[AsChoice]
public static partial class OrderProducts
{
    public interface IOrderProducts { }
    
    public record ValidatedOrderedProducts : IOrderProducts
    {
        public ValidatedOrderedProducts(IReadOnlyCollection<ValidatedOrderProduct> validatedProducts)
        {
            ValidatedProducts = validatedProducts;
        }
        public IReadOnlyCollection<ValidatedOrderProduct> ValidatedProducts { get; }
        
    }
}