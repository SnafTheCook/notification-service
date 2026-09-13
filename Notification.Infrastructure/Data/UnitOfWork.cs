using Notification.Domain.Interfaces;

namespace Notification.Infrastructure.Data
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public async Task SaveChangesAsync(CancellationToken ct) => await context.SaveChangesAsync(ct);

        public async Task BeginTransactionAsync(CancellationToken ct) => await context.Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct) => await context.Database.CommitTransactionAsync(ct);

        public async Task RollbackTransactionAsync(CancellationToken ct) => await context.Database.RollbackTransactionAsync(ct);
    }
}
