using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Achievements.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Achievements.Data.Mongodb.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public AchievementRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task<Models.Achievement[]> SelectAllAsync(Expression<Func<Models.Achievement, bool>> expression)
        {
            return await _mongoDbContext.Achievements.Where(expression).ToArrayAsync();
        }

        public async Task<Models.Achievement?> SelectFirstAsync(Expression<Func<Models.Achievement, bool>> expression)
        {
            return await _mongoDbContext.Achievements.FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(Models.Achievement achievement)
        {
            _mongoDbContext.Achievements.Add(achievement);

            await _mongoDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Models.Achievement achievement)
        {
            _mongoDbContext.Achievements.Remove(achievement);

            await _mongoDbContext.SaveChangesAsync();
        }
    }
}