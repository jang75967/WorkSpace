using CQRSExample.Models;
using MediatR;

namespace CQRSExample.Commands;

public class ProductCreatedNotification : INotification
{
    public Product Product { get; set; }
}
