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
        public CommandService commands { get; private set; }

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

            commands = new CommandService();
            CommandHandler commandHandler = new CommandHandler(client, commands);
            await commandHandler.StartCommandService();

            //Manage Events
            client.Log += m => { Console.WriteLine(m.Message); return Task.CompletedTask; };
            client.GuildAvailable += g => { Console.WriteLine($"Logging into {g.Name}"); return Task.CompletedTask; };
            client.Ready += () => { Console.WriteLine("Client ready..."); return Task.CompletedTask; };

            //Set Bot Presence
            await client.SetGameAsync($"{ConfigLoader.Prefix}help");

            //Logs into the bot using the Config Token and starts the bot
            await client.LoginAsync(TokenType.Bot, ConfigLoader.Token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
