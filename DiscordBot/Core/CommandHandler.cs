using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Linq;

namespace DiscordBot.Core
{
    public class CommandHandler
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider serviceProvider;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
        {
            this.client = client;
            this.commands = commands;
            this.serviceProvider = serviceProvider;
        }

        public async Task StartCommandService()
        {
            await commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: serviceProvider);
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

            var result = await commands.ExecuteAsync(context, argPos, serviceProvider);

            if (!result.IsSuccess)
            {
                var eb = new EmbedBuilder();
                if(result.Error == CommandError.BadArgCount)
                {
                    eb = BotUtils.ErrorEmbed("Invalid Arguments",
                        $"You provided to many or to little, \n arguments please try running " +
                        $"**{ConfigLoader.Prefix}help** for command info");

                    await m.Channel.SendMessageAsync(embed: eb.Build());
                    return;
                }

                eb = BotUtils.ErrorEmbed("Error Running Command",
                    $"There was a problem running your command, \n" +
                    $"please try running **{ConfigLoader.Prefix}help** to see a list of commands");

                await m.Channel.SendMessageAsync(embed: eb.Build());
            }
        }
    }
}
