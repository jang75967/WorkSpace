using CQRSExample.Commands;
using CQRSExample.Repository;
using MediatR;


namespace CQRSExample.Handlers;

public class AddProductHandler : IRequestHandler<AddProductCommand, int>
{
    private readonly FakeDataStore _fakeDataStore;
    private readonly IMediator _mediator;

    public AddProductHandler(FakeDataStore fakeDataStore, IMediator mediator)
    {
        _mediator = mediator;
        _fakeDataStore = fakeDataStore;
    }

    async Task<int> IRequestHandler<AddProductCommand, int>.Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        await _fakeDataStore.AddProduct(request.Product);

        await _mediator.Publish(new ProductCreatedNotification() { Product = request.Product }, CancellationToken.None);

        return request.Product.Id;
    }
}
