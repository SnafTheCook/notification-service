using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                builder.Property(n => n.Recipient).IsRequired().HasConversion(
                    r => r.Value,
                    v => Recipient.Create(v, Domain.Enums.ChannelType.Sms));
                builder.Property(n => n.Content).IsRequired();
            });
        }
    }
}
