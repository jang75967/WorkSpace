using InfraStructrue.Data.Persistence;
using MediatR;

namespace WorkerService.Core.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly RedisService _redisService;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var typeName = typeof(TRequest);

            try
            {
                using (var transaction = _redisService.BeginTranscation())
                {
                    if (transaction == null)
                        throw new ArgumentNullException(nameof(transaction));

                    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.Id, typeName, request);

                    var nextResult = await next();

                    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.Id, typeName);

                    await _redisService.Execute();

                    return nextResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
