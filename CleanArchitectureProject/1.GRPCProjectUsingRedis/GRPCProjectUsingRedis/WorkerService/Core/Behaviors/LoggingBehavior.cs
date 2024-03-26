using MediatR;
using System.Reflection;

namespace WorkerService.Core.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> // : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            //Request
            _logger.LogDebug($"Handling {typeof(TRequest).Name}");
            Type type = request.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                if ("Id" != property.Name)
                    continue;

                object propValue = property.GetValue(request, null)!;
                _logger.LogDebug($"{property.Name}: {propValue}");
            }

            var response = await next();

            //Response
            _logger.LogDebug($"Handled {typeof(TResponse).Name}");
            return response;
        }
    }
}
