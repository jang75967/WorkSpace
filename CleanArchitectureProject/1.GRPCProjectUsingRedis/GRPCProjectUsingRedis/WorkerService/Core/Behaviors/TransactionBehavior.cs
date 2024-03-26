using Application;
using MediatR;

namespace WorkerService.Core.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly IQueueService _queueService;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var typeName = typeof(TRequest);

            try
            {
                using (var transaction = _queueService.BeginTranscationAsync())
                {
                    if (transaction == null)
                        throw new ArgumentNullException(nameof(transaction));

                    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.Id, typeName, request);

                    var nextResult = await next();

                    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.Id, typeName);

                    await _queueService.ExecuteAsync();

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
