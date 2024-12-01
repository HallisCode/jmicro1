using System.Collections.Generic;
using System.Linq;
using Achievements.Contracts.Models;
using Telegram.Bot.Types;

namespace Achievements.Contracts.Output
{
    public record DisplayAchievementList
    {
        public Update Update { get; init; }
        public Achievement[] Achievements { get; init; }

        public DisplayAchievementList()
        {
        }

        public DisplayAchievementList
        (
            IEnumerable<Achievement> achievements,
            Update update
        )
        {
            Update = update;

            Achievements = achievements.ToArray();
        }
    }
}