using Achievements.Configuration;
using Achievements.Data.IRepositories;
using Achievements.Data.Mongodb.Repositories;
using Achievements.Services.Abstractions;
using Achievements.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Achievements
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            // DI: MassTransit (RabbitMQ)
            builder.Services.AddConfiguredMassTransit(
                builder.Configuration["RabbitMQ:host"]!,
                builder.Configuration["RabbitMQ:username"]!,
                builder.Configuration["RabbitMQ:password"]!
            );

            // DI: Repositories
            builder.Services.AddScoped<IUserChatStatsRepository, UserChatsRepository>();
            builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();

            // DI: Services
            builder.Services.AddScoped<IUserChatStatsService, UserChatStatsService>();
            builder.Services.AddScoped<IAchievementService, AchievementService>();

            // DI: MongoDb
            builder.Services.AddConfiguredMongoDb(
                builder.Configuration["MongoDb:connectionString"]!,
                builder.Configuration["MongoDb:dbName"]!
            );


            var app = builder.Build();

            // Add routing for API
            // app.MapControllers();

            app.Run();
        }
    }
}