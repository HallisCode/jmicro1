using System.Threading.Tasks;
using Achievements.Data.IRepositories;
using Achievements.Models;
using Achievements.Services.Abstractions;

namespace Achievements.Services.Implementations
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementService(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        public async Task<Models.Achievement> CreateAchievementAsync(long chatId, string title, long messages)
        {
            Models.Achievement achievement = new Models.Achievement(
                chatId: chatId,
                title: title,
                messages: messages
            );

            await _achievementRepository.CreateAsync(achievement);

            return achievement;
        }

        public async Task DeleteAchievementAsync(long chatId, string title)
        {
            Models.Achievement? achievement = await _achievementRepository.SelectFirstAsync(
                entity => entity.ChatId == chatId && entity.Title == title
            );

            if (achievement is not null)
            {
                await _achievementRepository.DeleteAsync(achievement);
            }
        }

        public async Task<Models.Achievement[]> SelectAllAchievementsAsync(long chatId)
        {
            return await _achievementRepository.SelectAllAsync(entity => entity.ChatId == chatId);
        }

        public async Task<Models.Achievement?> HasBeenAchievedAsync(UserChatStats userChatStats)
        {
            Models.Achievement[] achievements = await _achievementRepository.SelectAllAsync(
                entity => entity.ChatId == userChatStats.ChatId
            );

            foreach (Models.Achievement achievement in achievements)
            {
                if (achievement.Messages == userChatStats.Messages)
                {
                    return achievement;
                }
            }

            return null;
        }
    }
}