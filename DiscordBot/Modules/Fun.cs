using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot;

namespace DiscordBot.Modules
{
    public class Fun : ModuleBase<SocketCommandContext>
    {

        [Command("hello")]
        public async Task Hello()
        {
            await Context.Channel.SendMessageAsync("Hello World!");
        }

        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync($"Pong **{Program.bot.client.Latency}ms**");
        }
    }
}
