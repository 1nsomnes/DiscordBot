using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules
{
    [InitializeCommands("Admin Utilities")]
    [HelpModule("Admin Utilities", "Utilities for admins", Discord.GuildPermission.Administrator)]
    public class AdminUtilities : ModuleBase<SocketCommandContext>
    {
        [Command("changeprefix")]
        [Alias("prefix")]
        [CommandData("prefix <prefix>", "Change the prefix of the bot")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task ChangePrefix(string prefix)
        {
            JSONWrapper w = ConfigLoader.LoadData();
            w.data.prefix = prefix;
            ConfigLoader.SaveData(w);

            await Program.bot.client.SetGameAsync($"{ConfigLoader.Prefix}help");

            await ReplyAsync($"Prefix updated to `{prefix}`");

        }

        [Command("stopbot")]
        [Alias("stop")]
        [CommandData("stopbot", "Disconnects the bot")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task StopBot()
        {
            await ReplyAsync($"Disabling bot...");

            await Program.bot.client.StopAsync();

        }

        [Command("changestatus")]
        [Alias("status")]
        [CommandData("changestatus <status>", "Change the status of the bot")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task ChangeStatus(params string[] status)
        {
            await Program.bot.client.SetGameAsync($"{String.Join(" ", status)}");

            await ReplyAsync($"Updated presence to ***{String.Join(" ", status)}***");

        }
    }
}
