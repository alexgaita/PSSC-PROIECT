using Domain.Models;

namespace Domain.Commands;

public record PlaceOrderCommand
{
    public PlaceOrderCommand(string address, string email, IReadOnlyCollection<UnvalidatedOrderProduct> products)
    {
        Address = address;
        Email = email;
        UnvalidatedProducts = products;
    }
    
    public string Address { get; }
    public string Email { get; }
    public IReadOnlyCollection<UnvalidatedOrderProduct> UnvalidatedProducts { get; }
   
};