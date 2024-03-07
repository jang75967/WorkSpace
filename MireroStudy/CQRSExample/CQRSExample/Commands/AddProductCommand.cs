using CQRSExample.Models;
using LanguageExt;
using MediatR;

namespace CQRSExample.Commands;

public record AddProductCommand(Product Product) : IRequest<Option<Product>>;
