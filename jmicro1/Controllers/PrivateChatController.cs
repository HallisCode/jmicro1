using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramWebApplication.Infrastructure.Routing.Attributes;
using TelegramWebApplication.Infrastructure.Routing.Filters;

namespace jmicro1.Controllers
{
    [TelegramRoute]
    [PrivateChatFilter]
    public class PrivateChatController
    {
        public PrivateChatController()
        {
        }

        [TelegramRoute]
        public Task Handle(Update update)
        {
            Console.WriteLine("Handle is done.");

            return Task.CompletedTask;
        }
        
        [TelegramRoute]
        public Task Handle(Update update)
        {
            Console.WriteLine("Handle is done.");

            return Task.CompletedTask;
        }
    }
}