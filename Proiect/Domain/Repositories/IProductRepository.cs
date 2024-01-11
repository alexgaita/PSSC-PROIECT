using Domain.Models;
using LanguageExt;

namespace Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsByIds(IEnumerable<int> productIds);
    
    Task UpdateProductsStock(IEnumerable<ValidatedOrderProduct> products);
}