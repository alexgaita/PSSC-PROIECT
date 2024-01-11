using CSharp.Choices;

namespace Domain.Models;

[AsChoice]
public static partial class PlacedOrder
{
    public interface IPlacedOrder { }
    
   public record UnvalidatedPlacedOrder : IPlacedOrder
    {
        public UnvalidatedPlacedOrder(IReadOnlyCollection<UnvalidatedOrderProduct> unvalidatedProducts, string address, string email)
        {
            UnvalidatedProducts = unvalidatedProducts;
            Address = address;
            Email = email;
        }
        public IReadOnlyCollection<UnvalidatedOrderProduct> UnvalidatedProducts { get; }
        public string Address { get; }
        public string Email { get; }
        
    }
    
    public record InvalidPlacedOrder : IPlacedOrder
    {
        public string Reason { get; }

        public InvalidPlacedOrder(string reason)
        {
            Reason = reason;
        }
    }
    
    public record NotCalculatedOrder : IPlacedOrder
    {
        public  NotCalculatedOrder(IReadOnlyCollection<ValidatedOrderProduct> products, string address, string email)
        {
            Products = products;
            Address = address;
            Email = email;
        }
        
        public string Address { get; }
        public string Email { get; }
        public IReadOnlyCollection<ValidatedOrderProduct> Products { get; }
    }

    public record CalculatedOrder : IPlacedOrder
    {
        public CalculatedOrder(IReadOnlyCollection<ValidatedOrderProduct> validatedProducts, string address, string email, int total)
        {
            ValidatedProducts = validatedProducts;
            Address = address;
            Email = email;
            Total = total;
        }
        
        public string Address { get; }
        public string Email { get; }
        public IReadOnlyCollection<ValidatedOrderProduct> ValidatedProducts { get; }
        public int Total { get; }
    }
    
    public record PublishedOrder : IPlacedOrder
    {
        public PublishedOrder(IReadOnlyCollection<ValidatedOrderProduct> validatedProducts, string address, string email, int total, int orderId)
        {
            ValidatedProducts = validatedProducts;
            Address = address;
            Email = email;
            Total = total;
            OrderId = orderId;
        }
        
        public string Address { get; }
        public string Email { get; }
        public IReadOnlyCollection<ValidatedOrderProduct> ValidatedProducts { get; }
        public int Total { get; }
        public int OrderId { get; }
    }
        
}