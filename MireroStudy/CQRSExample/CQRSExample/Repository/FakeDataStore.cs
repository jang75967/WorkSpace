using CQRSExample.Models;

namespace CQRSExample.Repository;

public class FakeDataStore
{
    private static List<Product> _products = default!;
    public FakeDataStore() => _products =
        [
            new Product { Id = 1, Name = "Test Product 1" },
            new Product { Id = 2, Name = "Test Product 2" },
            new Product { Id = 3, Name = "Test Product 3" }
        ];
    public async Task AddProduct(Product product)
    {
        _products.Add(product);
        await Task.CompletedTask;
    }

    public async Task DeleteProduct(int id)
    {
        var productToRemove = _products.FirstOrDefault(p => p.Id == id);
        if (productToRemove != null)
        {
            _products.Remove(productToRemove);
        }
        else
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Product>> GetAllProducts() => await Task.FromResult(_products);

    public async Task<Product> GetProductById(int id) => await Task.FromResult(_products.SingleOrDefault(p => p.Id == id)!);
}
