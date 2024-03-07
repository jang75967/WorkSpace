using CQRSExample.Commands;
using CQRSExample.Models;
using CQRSExample.Repository;
using LanguageExt;
using MediatR;


namespace CQRSExample.Handlers;

public class AddProductHandler : IRequestHandler<AddProductCommand, Option<Product>>
{
    private readonly FakeDataStore _fakeDataStore;
    private readonly IMediator _mediator;

    public AddProductHandler(FakeDataStore fakeDataStore, IMediator mediator)
    {
        _mediator = mediator;
        _fakeDataStore = fakeDataStore;
    }

    async Task<Option<Product>> IRequestHandler<AddProductCommand, Option<Product>>.Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        await _fakeDataStore.AddProduct(request.Product);

        await _mediator.Publish(new ProductCreatedNotification() { Product = request.Product }, CancellationToken.None);

        return Option<Product>.Some(request.Product);
    }
}
