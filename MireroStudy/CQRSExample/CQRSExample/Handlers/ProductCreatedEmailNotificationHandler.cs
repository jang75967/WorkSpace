using CQRSExample.Commands;
using MediatR;

namespace CQRSExample.Handlers;

public class ProductCreatedEmailNotificationHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger<ProductCreatedEmailNotificationHandler> _logger;

    public ProductCreatedEmailNotificationHandler(ILogger<ProductCreatedEmailNotificationHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Confirmation email sent for product {notification.Product.Id}");
        return Task.CompletedTask;
    }
}
