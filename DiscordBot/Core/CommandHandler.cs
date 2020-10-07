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
            //Gets all assemblies in Modules
            /*var assembelies = Assembly.GetEntryAssembly().GetTypes().
                Where(x => x.Namespace.Equals("DiscordBot.Modules") && x.GetTypeInfo().IsClass);
            foreach(var x in assembelies)
            {
                Console.WriteLine($"Loading {x.Name} module, Type: {x.GetType()}, Is Class: {x.GetTypeInfo().IsClass}");
                await commands.AddModulesAsync(assembly: x.Assembly, services: serviceProvider);
            }*/

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
                    eb = new EmbedBuilder()
                    {
                        Title = "Invalid Arguments",

                        Description = $"You provided to many or to little, \n" +
                    $"arguments please try running **{ConfigLoader.Prefix}help** for command info",

                        Timestamp = DateTime.UtcNow,

                        Color = new Color(255, 0, 0)
                    };
                    await m.Channel.SendMessageAsync(embed: eb.Build());
                    return;
                }
                eb = new EmbedBuilder()
                {
                    Title = "Error Running Command",

                    Description = $"There was a problem running your command, \n" +
                    $"please try running **{ConfigLoader.Prefix}help** to see a list of commands",

                    Timestamp = DateTime.UtcNow,

                    Color = new Color(255, 0, 0)
                };

                await m.Channel.SendMessageAsync(embed: eb.Build());
            }
        }
    }
}
