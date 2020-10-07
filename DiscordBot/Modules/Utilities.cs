using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Modules
{
    [Core.CustomModule("Utilities", "Quality of life commands that will inhance your discord experience.")]
    public class Utilities : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync($"Pong **{Program.bot.client.Latency}ms**");
        }
    }
}
