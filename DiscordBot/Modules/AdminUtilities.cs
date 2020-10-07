using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules
{
    public class AdminUtilities : ModuleBase<SocketCommandContext>
    {
        [Command("changeprefix")]
        [Alias("prefix")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task ChangePrefix(string prefix)
        {
            JSONWrapper w = ConfigLoader.LoadData();
            w.data.prefix = prefix;
            ConfigLoader.SaveData(w);

            await Program.bot.client.SetGameAsync($"{ConfigLoader.Prefix}help");

            await ReplyAsync($"Prefix updated to {prefix}");

        }

        [Command("stopbot")]
        [Alias("stop")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task StopBot()
        {
            await ReplyAsync($"Disabling bot...");

            await Program.bot.client.StopAsync();

        }

        [Command("changestatus")]
        [Alias("status")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task ChangeStatus(params string[] status)
        {
            await Program.bot.client.SetGameAsync($"{String.Join(" ", status)}");

            await ReplyAsync($"Updated presence to ***{String.Join(" ", status)}***");

        }
    }
}
