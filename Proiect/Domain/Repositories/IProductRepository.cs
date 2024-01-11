using Domain.Models;
using LanguageExt;

namespace Domain.Repositories;

public interface IProductRepository
{
    TryAsync<List<Product>> GetAllProductsByIds(IEnumerable<int> productIds);
    
    // TryAsync<Unit> UpdateProductsStock(IEnumerable<ValidatedOrderProduct> products);
}