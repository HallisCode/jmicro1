using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Achievements.Models
{
    public class UserChatStats
    {
        public ObjectId Id { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }

        public List<string> Awards { get; set; }
        public long Messages { get; set; }

        public UserChatStats(
            long chatId,
            long userId,
            long messages = 0,
            List<string>? awards = null)
        {
            ChatId = chatId;
            UserId = userId;
            Messages = messages;

            if (awards is null)
            {
                awards = new List<string>();
            }

            Awards = awards;
        }
    }
}