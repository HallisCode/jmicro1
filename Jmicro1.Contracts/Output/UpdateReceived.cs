using System;
using MassTransit;
using Telegram.Bot.Types;

namespace Jmicro1.Contracts.Output
{
    public record UpdateReceived
    {
        public Update Update { get; init; }
        public bool IsCommand { get; init; }
        public string Command { get; init; } = "";
        public string[] CommandArgs { get; init; } = Array.Empty<string>();

        public UpdateReceived()
        {
        }

        public UpdateReceived(Update update)
        {
            Update = update;
            IsCommand = false;
        }

        public UpdateReceived
        (
            Update update,
            string command,
            string?[] commandArgs = null
        )
        {
            Update = update;
            IsCommand = true;
            Command = command;
            CommandArgs = (commandArgs ?? CommandArgs)!;
        }
    }
}