using Domain.Models;
using Domain.Repositories;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using TakeCommand.Data.Models;

namespace TakeCommand.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopContext _dbContext;

    public ProductRepository(ShopContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<Product>> GetAllProductsByIds(IEnumerable<int> productIds)
    {
        var products = await _dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .AsNoTracking()
            .ToListAsync();

        return products.Select(product => new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock
        }).ToList();
    }
    
    public async Task UpdateProductsStock(IEnumerable<ValidatedOrderProduct> products)
    {
        var productDtos = products.Select(product => new ProductDto()
        {
            Id = product.Product.Id,
            Name = product.Product.Name,
            Price = product.Product.Price,
            Stock = product.Product.Stock - product.Quantity
        }).ToList();
        
        _dbContext.Products.UpdateRange(productDtos);

        await _dbContext.SaveChangesAsync();
    }





}