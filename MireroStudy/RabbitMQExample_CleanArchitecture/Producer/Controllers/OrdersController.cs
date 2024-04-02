using Application.MessageBus;
using Microsoft.AspNetCore.Mvc;
using Producer.Data;
using Producer.Dtos;

namespace Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FakeDataStore _context;
    private readonly IMessageProducer _messagePublisher;

    public OrdersController(FakeDataStore context, IMessageProducer messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderDto orderDto)
    {
        Order order = new()
        {
            ProductName = orderDto.ProductName,
            Price = orderDto.Price,
            Quantity = orderDto.Quantity
        };

        await _context.AddOrder(order);

        _messagePublisher.Publish(order);

        return Ok(new { id = order.Id });
    }
}