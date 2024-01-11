namespace Domain.Models;

public record UnvalidatedOrderProduct(int ProductId, int Quantity);

public record ValidatedOrderProduct(Product Product, int Quantity);