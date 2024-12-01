using Telegram.Bot.Types;

namespace Achievements.Contracts.Output
{
    public record AchievementAwarded
    {
        public Update Update { get; init; }
        public string Title { get; init; }
        public long Messages { get; init; }


        public AchievementAwarded()
        {
        }

        public AchievementAwarded
        (
            Update update,
            string title,
            long messages
            )
        {
            Update = update;
            Title = title;
            Messages = messages;
        }
    }
}