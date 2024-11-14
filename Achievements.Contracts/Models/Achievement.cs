namespace Achievements.Contracts.Models
{
    public record Achievement
    {
        public long ChatId { get; set; }
        public string Title { get; set; }
        public long Messages { get; set; }

        public Achievement()
        {
            
        }
        
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