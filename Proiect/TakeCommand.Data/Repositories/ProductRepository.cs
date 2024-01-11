using Domain.Models;
using Domain.Repositories;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;

namespace TakeCommand.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _dbContext;

    public ProductRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }


    public TryAsync<List<Product>> GetAllProductsByIds(IEnumerable<int> productIds) => async () =>
    {
        var products = await _dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .AsNoTracking()
            .ToListAsync();

        return products.Select(product => new Product()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        }).ToList();
    };





}