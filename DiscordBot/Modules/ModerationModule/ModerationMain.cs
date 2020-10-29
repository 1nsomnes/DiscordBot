using System;
using Discord.Commands;
using DiscordBot.Core;
using Discord;
using System.Threading.Tasks;
using DiscordBot.Core.Database;

namespace DiscordBot.Modules.ModerationModule
{
    [HelpModule("Moderation", "Commands for moderators", GuildPermission.ManageMessages)]
    public class ModerationMain : ModuleBase<SocketCommandContext>
    {
        [Command("newinf")]
        public async Task NewInfraction()
        {
            var i = DatabaseService.AddInfraction(1, "default", "default", 1);

            await ReplyAsync($"Created infraction with id {i.id}");
        }

        [Command("editinf"), Alias("modinf")]
        public async Task ModifyInfraction(ulong id, string description)
        {

        }
    }
}
