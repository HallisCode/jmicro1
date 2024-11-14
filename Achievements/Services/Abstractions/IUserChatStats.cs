using System.Threading.Tasks;
using Achievements.Models;

namespace Achievements.Services.Abstractions
{
    public interface IUserChatStatsService
    {
        Task<UserChatStats> IncrementMessageAsync(long chatId, long userId);
    }
}