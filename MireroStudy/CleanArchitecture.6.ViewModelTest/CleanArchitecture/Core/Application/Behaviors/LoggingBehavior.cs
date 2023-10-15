using MediatR;
using System.Reflection;

namespace CleanArchitecture.Core.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        //Request
        _logger.LogDebug($"Handling {typeof(TRequest).Name}");
        Type myType = request.GetType();

        foreach (PropertyInfo prop in myType.GetProperties())
        {
            if ("Id" != prop.Name)
                continue;

            object propValue = prop.GetValue(request, null)!;
            _logger.LogDebug("{Property} : {@Value}", prop.Name, propValue);
        }
        var response = await next();

        //Response
        _logger.LogDebug($"Handled {typeof(TResponse).Name}");
        return response;
    }
}