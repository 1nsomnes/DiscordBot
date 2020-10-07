using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules
{
    [CustomModule("Fun", "Commands that are fun!")]
    public class Fun : ModuleBase<SocketCommandContext>
    {

        [Command("hello")]
        public async Task Hello()
        {
            await Context.Channel.SendMessageAsync("Hello World!");
        }

    }
}
