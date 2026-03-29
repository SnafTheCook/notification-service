
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Interfaces;
using Notification.Infrastructure.BackgroundJobs;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Providers;
using Notification.Infrastructure.Repositories;
using Notification.Infrastructure.Services;
using Notification.Infrastructure.Settings;
using Scalar.AspNetCore;
using FluentValidation;

namespace Notification.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.Configure<NotificationSettings>(builder.Configuration.GetSection("NotificationSettings"));

            builder.Services.AddTransient<INotificationProvider, TwilioSmsProvider>();
            builder.Services.AddTransient<INotificationProvider, AmazonSnsEmailProvider>();
            builder.Services.AddTransient<INotificationProvider, VonageSmsProvider>();

            builder.Services.AddScoped<NotificationDispatcher>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            builder.Services.AddHostedService<RetryWorker>();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
