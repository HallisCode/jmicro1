using Achievements.Data.Mongodb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Achievements.Configuration
{
    public static class AddMongoDbExtension
    {
        public static void AddConfiguredMongoDb(
            this IServiceCollection services,
            string stringConnectMongoDb,
            string dbName)
        {
            IMongoClient mongoClient = new MongoClient(stringConnectMongoDb);

            InitializerMongoDb.Initialize(mongoClient);

            services.AddDbContext<MongoDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseMongoDB(mongoClient, dbName);
            });
        }
    }
}