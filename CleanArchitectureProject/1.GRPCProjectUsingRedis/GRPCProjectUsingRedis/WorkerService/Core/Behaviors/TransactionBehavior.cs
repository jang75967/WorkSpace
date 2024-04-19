using Application.Persistences;
using MediatR;

namespace WorkerService.Core.Behaviors
{
    public interface ILoggingTransaction { }

    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ILoggingTransaction
    {
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly IQueue _queue;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger, IQueue queue, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _queue = queue;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            
            var typeName = typeof(TRequest).Name;

            try
            {
                #region Queue Transaction Test
                //using (var transaction = _queue.BeginTransactionAsync())
                //{
                //    if (transaction is null)
                //        throw new ArgumentNullException(nameof(transaction));

                //    _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({Command})", transaction.Id, typeName, request);

                //    var nextResult = await next();

                //    _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.Id, typeName);

                //    await _queue.ExecuteAsync();

                //    return nextResult;
                //}
                #endregion

                using (var transaction = _unitOfWork.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        if (transaction is null)
                            throw new ArgumentNullException(nameof(transaction));

                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName}", transaction.Id, typeName);

                        var nextResult = await next();

                        await _unitOfWork.CommitAsync(cancellationToken);

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.Id, typeName);

                        return nextResult;
                    }
                    catch
                    {
                        // 한번은 Commit, 한번은 Rollback 테스트 ㄱㄱ
                        await _unitOfWork.CommitAsync(cancellationToken); // 테스트용

                        await _unitOfWork.RollbackAsync(cancellationToken); // 이거 주석처리 (테스트 해야됨)

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({Command})", typeName, request);
                throw;
            }
        }
    }
}
