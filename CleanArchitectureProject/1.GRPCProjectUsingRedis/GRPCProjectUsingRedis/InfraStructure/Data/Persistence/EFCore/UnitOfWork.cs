using Application.Persistences;
using Domain.Entities;
using InfraStructure.Data.Repositories;

namespace InfraStructure.Data.Persistence.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<User> UserRepository { get; private set; }
        public IBaseRepository<Group> GroupRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new EFCoreRepository<User>(context);
            GroupRepository = new EFCoreRepository<Group>(context);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _context.Database.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
