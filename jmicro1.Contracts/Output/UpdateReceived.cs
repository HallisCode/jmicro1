using MassTransit;
using Telegram.Bot.Types;

namespace jmicro1.Contracts.Output
{
    public record UpdateReceived
    {
        public Update Update { get; init; }
        public bool IsCommand { get; init; }
        public string Command { get; init; }
        public string[] CommandArgs { get; init; }
        
        public UpdateReceived()
        {
        }

        public UpdateReceived(Update update)
        {
            Update = update;
            IsCommand = false;
            Command = "";
            CommandArgs = new string[0];
        }

        public UpdateReceived(
            Update update,
            string command,
            string?[] commandArgs = null)
        {
            Update = update;
            IsCommand = true;
            Command = command;
            CommandArgs = (commandArgs ?? new string[0])!;
        }
    }
}