using Client.Business.Core.Domain.Attributes;
using Client.Business.Core.Domain.Events.Retry;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;
using Polly;
using System.Reflection;

namespace Client.Business.Core.Application.Behaviors;

public class RetryPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<RetryPolicyBehavior<TRequest, TResponse>> _logger;
    private readonly IMessenger _messenger;

    public RetryPolicyBehavior(ILogger<RetryPolicyBehavior<TRequest, TResponse>> logger, IMessenger messenger)
    {
        _logger = logger;
        _messenger = messenger;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var retryAttr = typeof(TRequest).GetCustomAttribute<RetryPolicyAttribute>();
        if (retryAttr == null)
        {
            return await next();
        }
        try
        {
            return await Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    retryAttr.RetryCount,
                    i => TimeSpan.FromMilliseconds(i * retryAttr.SleepDuration),
                    (ex, ts, _) => _logger.LogWarning(ex, "Failed to execute handler for request {Request}, retrying after {RetryTimeSpan}s: {ExceptionMessage}", typeof(TRequest).Name, ts.TotalSeconds, ex.Message))
                .ExecuteAsync(async () => await next());
        }
        catch
        {
            var @event = new RetryErrorEvent($"All retry attempts failed for request {typeof(TRequest).Name}");
            _messenger.Send(@event);
            return default(TResponse)!; 
        }
    }
}
