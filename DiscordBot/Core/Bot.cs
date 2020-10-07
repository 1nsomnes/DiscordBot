using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;

namespace DiscordBot.Core
{
    public class Bot
    {
        public DiscordSocketClient client { get; private set; }
        public CommandService command { get; private set; }

        public async Task RunBot()
        {
            //Load Custom Config Code
            ConfigLoader.Load();

            //Check if there is a prefix actually set
            if (ConfigLoader.Prefix.Equals("") || ConfigLoader.Token.Equals(""))
            {
                Console.WriteLine("Token and/or Prefix not set in config.json");
                return;
            }

            //Generates the config for the client
            var config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info
            };

            client = new DiscordSocketClient(config);

            command = new CommandService();
            await command.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);

            //Manage Events
            client.Log += m => { Console.WriteLine(m.Message); return Task.CompletedTask; };
            client.GuildAvailable += g => { Console.WriteLine($"Logging into {g.Name}"); return Task.CompletedTask; };
            client.Ready += () => { Console.WriteLine("Client ready..."); return Task.CompletedTask; };
            client.MessageReceived += MessageHandler;

            //Set Bot Presence
            await client.SetGameAsync($"{ConfigLoader.Prefix}help");

            //Logs into the bot using the Config Token and starts the bot
            await client.LoginAsync(TokenType.Bot, ConfigLoader.Token);
            await client.StartAsync();

            await Task.Delay(-1);
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

            var result = await command.ExecuteAsync(context, argPos, null);

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
