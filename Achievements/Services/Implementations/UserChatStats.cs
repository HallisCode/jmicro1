using System.Threading.Tasks;
using Achievements.Data.IRepositories;
using Achievements.Models;
using Achievements.Services.Abstractions;

namespace Achievements.Services.Implementations
{
    public class UserChatStatsService : IUserChatStatsService
    {
        private readonly IUserChatStatsRepository _userChatStatsRepository;


        public UserChatStatsService(
            IUserChatStatsRepository userChatStatsRepository
        )
        {
            _userChatStatsRepository = userChatStatsRepository;
        }

        public async Task<UserChatStats> IncrementMessageAsync(long chatId, long userId)
        {
            var userChatStats = await _userChatStatsRepository.SelectFirstAsync(
                userChatStats => userChatStats.ChatId == chatId && userChatStats.UserId == userId
            );

            // Создаём новую запись статистики
            if (userChatStats is null)
            {
                var newUserChatStat = new UserChatStats(
                    chatId: chatId,
                    userId: userId,
                    messages: 1
                );

                await _userChatStatsRepository.CreateAsync(newUserChatStat);

                return newUserChatStat;
            }

            // ОБновляем статистику
            userChatStats.Messages += 1;

            await _userChatStatsRepository.UpdateAsync(userChatStats);

            return userChatStats;
        }
    }
}