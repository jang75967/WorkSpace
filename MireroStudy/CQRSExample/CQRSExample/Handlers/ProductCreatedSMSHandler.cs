using CQRSExample.Commands;
using MediatR;

namespace CQRSExample.Handlers;

public class ProductCreatedSMSHandler : INotificationHandler<ProductCreatedNotification>
{
    private readonly ILogger<ProductCreatedSMSHandler> _logger;

    public ProductCreatedSMSHandler(ILogger<ProductCreatedSMSHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Send a confirmation email
        _logger.LogInformation($"Confirmation sms sent for product {notification.Product.Id}");

        return Task.CompletedTask;
    }
}
