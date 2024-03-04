using CQRSExample.Commands;
using CQRSExample.Repository;
using MediatR;

namespace CQRSExample.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
{
    private readonly FakeDataStore _fakeDataStore;

    public DeleteProductCommandHandler(FakeDataStore fakeDataStore) => _fakeDataStore = fakeDataStore;

    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var deleteProduct = _fakeDataStore.GetProductById(request.Id);

        if (deleteProduct is null)
            return default!;

        await _fakeDataStore.DeleteProduct(request.Id);

        return deleteProduct.Result.Id;
    }
}
