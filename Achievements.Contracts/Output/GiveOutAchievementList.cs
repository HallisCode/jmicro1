using System.Collections.Generic;
using System.Linq;
using Achievements.Contracts.Models;
using Achievements.Contracts.Models.Output;
using Telegram.Bot.Types;

namespace Achievements.Contracts.Output
{
    public record GiveOutAchievementList
    {
        public Update Update { get; set; }
        
        public Achievement[] Achievements { get; set; }

        public GiveOutAchievementList()
        {
        }

        public GiveOutAchievementList(
            IEnumerable<Achievement> achievements,
            Update update)
        {
            Update = update;
            
            Achievements = achievements.ToArray();
        }
    }
}