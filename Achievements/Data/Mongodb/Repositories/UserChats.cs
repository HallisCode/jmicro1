using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Achievements.Data.IRepositories;
using Achievements.Data.Mongodb;
using Achievements.Models;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Data.Mongodb.Repositories
{
    public class UserChatsRepository : IUserChatStatsRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public UserChatsRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<UserChatStats?> SelectFirstAsync(Expression<Func<UserChatStats, bool>> expression)
        {
            return await _mongoDbContext.UserChatStats.FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(UserChatStats userChatStats)
        {
            _mongoDbContext.Add(userChatStats);

            await _mongoDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserChatStats userChatStats)
        {
            _mongoDbContext.Update(userChatStats);

            await _mongoDbContext.SaveChangesAsync();
        }
    }
}