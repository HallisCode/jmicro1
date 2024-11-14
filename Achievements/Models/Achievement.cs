using MongoDB.Bson;

namespace Achievements.Models
{
    public class Achievement
    {
        public ObjectId Id { get; set; }

        public long ChatId { get; set; }
        public string Title { get; set; }
        public long Messages { get; set; }

        public Achievement(
            long chatId,
            string title,
            long messages
        )
        {
            ChatId = chatId;
            Title = title;
            Messages = messages;
        }
    }
}