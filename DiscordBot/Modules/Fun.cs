using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;
using Discord;

namespace DiscordBot.Modules
{
    [InitializeCommands("Fun")]
    [HelpModule("Fun", "Commands that are fun!")]
    public class Fun : ModuleBase<SocketCommandContext>
    {

        [Command("hello")]
        [CommandData("hello")]
        public async Task Hello(params string[] args)
        {
            await Context.Channel.SendMessageAsync("Hello World!");
        }

        [Command("F")]
        [CommandData("F", "F")]
        public async Task F(params string[] args)
        {
            
            var message = await ReplyAsync("F");
            await message.AddReactionAsync(new Emoji("\uD83C\uDDEB"));
        }

        [Command("whenschristmas")]
        [Alias("christams", "christmastime", "timechristmas", "daystillchristmas", "whenchristmas", "snow")]
        [CommandData("whenschristmas", "Self explanatory")]
        public async Task Christmas(params string[] args)
        {
            DateTime today = DateTime.UtcNow;
            DateTime next = new DateTime(today.Year, 12, 25);

            if (next < today) next.AddYears(1);

            var days = (next - today).Days;

            await ReplyAsync($"**{days}** days until Christmas!");
        }
    }
}
