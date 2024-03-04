using CQRSExample.Models;
using MediatR;

namespace CQRSExample.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Product>;