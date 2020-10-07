using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;

namespace DiscordBot.Core
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.client = client;
            this.commands = commands;
        }

        public async Task StartCommandService()
        {
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
            client.MessageReceived += MessageHandler;
        }

        private async Task MessageHandler(SocketMessage m)
        {
            var message = m as SocketUserMessage;
            if (message is null) return;

            int argPos = 0;

            if (!(message.HasStringPrefix(ConfigLoader.Prefix, ref argPos) ||
            message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
            message.Author.IsBot) return;

            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess)
            {
                Console.WriteLine("Unsuccesful result");
                var eb = new EmbedBuilder()
                {
                    Title = "Error running command",

                    Description = $"There was a problem running your command, \n" +
                    $"please try running **{ConfigLoader.Prefix}help** to see a list commands",

                    Timestamp = DateTime.UtcNow,

                    Color = new Color(255, 0, 0)
                };

                await m.Channel.SendMessageAsync(embed: eb.Build());
            }
        }
    }
}
