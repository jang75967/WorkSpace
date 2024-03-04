using CQRSExample.Commands;
using CQRSExample.Repository;
using MediatR;

namespace CQRSExample.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly FakeDataStore _fakeDataStore;

    public UpdateProductCommandHandler(FakeDataStore fakeDataStore) => _fakeDataStore = fakeDataStore;

    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var updateProduct = await _fakeDataStore.GetProductById(request.Id);

        if (updateProduct is null)
            return default!;

        updateProduct.Name = request.Name;

        return updateProduct.Id;
    }
}
