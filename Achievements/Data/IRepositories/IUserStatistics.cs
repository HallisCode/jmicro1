using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Achievements.Models;

namespace Achievements.Data.IRepositories
{
    public interface IUserChatStatsRepository
    {
        Task<UserChatStats?> SelectFirstAsync(Expression<Func<UserChatStats, bool>> expression);

        Task CreateAsync(UserChatStats userChatStats);

        Task UpdateAsync(UserChatStats userChatStats);
    }
}