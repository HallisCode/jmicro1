using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Achievements.Data.IRepositories
{
    public interface IAchievementRepository
    {
        Task<Models.Achievement[]> SelectAllAsync(Expression<Func<Models.Achievement, bool>> expression);

        Task<Models.Achievement?> SelectFirstAsync(Expression<Func<Models.Achievement, bool>> expression);

        Task CreateAsync(Models.Achievement achievement);

        Task DeleteAsync(Models.Achievement achievement);
    }
}