using System.Threading.Tasks;
using Achievements.Models;

namespace Achievements.Services.Abstractions
{
    public interface IAchievementService
    {
        Task<Models.Achievement> CreateAchievementAsync(long chatId, string title, long messages);

        Task DeleteAchievementAsync(long chatId, string title);

        Task<Models.Achievement[]> SelectAllAchievementsAsync(long chatId);

        Task<Models.Achievement?> HasBeenAchievedAsync(UserChatStats userChatStats);
    }
}