namespace Producer.Data;

public class FakeDataStore
{
    private static List<Order> _orders = default!;
    public FakeDataStore()
    {
        _orders = new List<Order>()
        {
            new Order { Id = 1, Price = 1000, ProductName = "라면", Quantity = 1 },
            new Order { Id = 2, Price = 1000, ProductName = "라면2", Quantity = 1 },
            new Order { Id = 3, Price = 1000, ProductName = "라면3", Quantity = 1 }
        };
    }

    public async Task AddOrder(Order order)
    {
        _orders.Add(order);
        await Task.CompletedTask;
    }

    public async Task DeleteProduct(int id)
    {
        var orderToRemove = _orders.FirstOrDefault(p => p.Id == id);
        if (orderToRemove != null)
        {
            _orders.Remove(orderToRemove);
        }
        else
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<Order>> GetAllOrders() => await Task.FromResult(_orders);

    public async Task<Order> GetOrderById(int id) => await Task.FromResult(_orders.SingleOrDefault(p => p.Id == id)!);
}
