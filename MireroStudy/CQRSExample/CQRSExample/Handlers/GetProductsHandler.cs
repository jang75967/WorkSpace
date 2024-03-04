using CQRSExample.Models;
using CQRSExample.Queries;
using CQRSExample.Repository;
using MediatR;

namespace CQRSExample.Handlers;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly FakeDataStore _fakeDataStore;
    public GetProductsHandler(FakeDataStore fakeDataStore) => _fakeDataStore = fakeDataStore;
    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken) => await _fakeDataStore.GetAllProducts();
}
