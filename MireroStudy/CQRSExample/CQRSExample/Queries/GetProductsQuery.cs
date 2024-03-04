using CQRSExample.Models;
using MediatR;

namespace CQRSExample.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<Product>>;
