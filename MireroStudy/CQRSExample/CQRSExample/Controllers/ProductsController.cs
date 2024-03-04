using CQRSExample.Commands;
using CQRSExample.Models;
using CQRSExample.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSExample.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _mediator.Send(new GetProductsQuery());
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct([FromBody] Product product)
    {
        await _mediator.Send(new AddProductCommand(product));
        return StatusCode(201);
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<ActionResult> GetProductById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }

    [HttpPut]
    public async Task<ActionResult> Update(Product product)
    {
        await _mediator.Send(new UpdateProductCommand(product.Id, product.Name));
        return StatusCode(201);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return StatusCode(204);
    }
}
