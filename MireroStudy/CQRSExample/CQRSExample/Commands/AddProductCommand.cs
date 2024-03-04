using CQRSExample.Models;
using MediatR;

namespace CQRSExample.Commands;

public record AddProductCommand(Product Product) : IRequest<int>;
