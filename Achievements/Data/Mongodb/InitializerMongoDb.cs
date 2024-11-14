using Achievements.Models;
using MongoDB.Driver;

namespace Achievements.Data.Mongodb
{
    public static class InitializerMongoDb
    {
        public static void Initialize(IMongoClient mongoClient)
        {
            var collectionUserChatStats = mongoClient.GetDatabase("achievements")
                .GetCollection<UserChatStats>("user_chat_stats");

            var collectionChatAchievements = mongoClient.GetDatabase("achievements")
                .GetCollection<Models.Achievement>("chat_achievements");


            var indexUniqueOptions = new CreateIndexOptions() { Unique = true };

            // Настраиваем уникальный индекс на <user_id>.<chat_id> collection user_chat_stats
            var indexUserChatStats = new CreateIndexModel<UserChatStats>(Builders<UserChatStats>.IndexKeys
                    .Ascending(entity => entity.UserId)
                    .Ascending(entity => entity.ChatId),
                indexUniqueOptions
            );
            collectionUserChatStats.Indexes.CreateOne(indexUserChatStats);

            // Настраиваем уникальный индекс на <chat_id>.<title> collection chat_achievements
            var indexAchievement = new CreateIndexModel<Achievement>(Builders<Achievement>.IndexKeys
                    .Ascending(entity => entity.ChatId)
                    .Ascending(entity => entity.Title),
                indexUniqueOptions
            );
            collectionChatAchievements.Indexes.CreateOne(indexAchievement);
        }
    }
}