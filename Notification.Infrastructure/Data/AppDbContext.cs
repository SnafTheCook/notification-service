using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<NotificationEntity> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotificationEntity>(builder =>
            {
                builder.HasKey(n => n.Id);
                builder.Property(n => n.Recipient).IsRequired();
                builder.Property(n => n.Content).IsRequired();
            });
        }
    }
}
