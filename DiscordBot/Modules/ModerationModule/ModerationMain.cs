using System;
using Discord.Commands;
using DiscordBot.Core;
using Discord;
using System.Threading.Tasks;
using DiscordBot.Core.Database;

namespace DiscordBot.Modules.ModerationModule
{
    [InitializeCommands("Moderation")]
    [HelpModule("Moderation", "Commands for moderators", GuildPermission.ManageMessages)]
    public class ModerationMain : ModuleBase<SocketCommandContext>
    {

        public enum PunishmentType { Warned, TempMuted, Muted, TempBanned, Banned };

        [Command("warn")]
        [CommandData("warn", "Warn a user")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task WarnUser(ulong id, params string[] reason)
        {
            var i = DatabaseService.AddInfraction(id, "warn", string.Join(" ", reason), Context.User.Id);

            var user = (IGuildUser)Context.Guild.GetUser(id);

            var embed = new EmbedBuilder().
                WithTitle($"[Warned User] {id}").
                AddField("Reason", string.Join(" ", reason)).
                WithTimestamp(DateTime.UtcNow).
                WithFooter("INF ID: " + i.id).
                WithColor(BotColors.ORANGE);

            if (!(user is null))
            {
                embed.Title = "";
                embed.Author = new EmbedAuthorBuilder().WithIconUrl(user.GetAvatarUrl()).WithName("[Warned User] " + user.Tag());
            }

            await ReplyAsync(embed: embed.Build());
        }

        public void CarryOutPunishment(ulong id, PunishmentType punishmentType)
        {

        }

        [Command("infraction"), Alias("inf", "infr")]
        [CommandData("infraction", "View data on an infraction")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Infraction(ulong id)
        {
            var i = DatabaseService.GetInfraction(id);

            if (i == null)
            {
                var error = BotUtils.ErrorEmbed(description: "Could not find specified infraction id", withTimestamp: false);
                await ReplyAsync(embed: error.Build());
                return;
            }

            var embed = new EmbedBuilder().
                WithTitle($"Infraction {i.id} Data").
                AddField("User ID", i.userId, true).
                AddField("Mod ID", i.modId, true).
                AddField("Severity", i.severity, true).
                AddField("\nCreation Date", i.creationDate, true).
                AddField("Modify Date", i.modificationDate, true).
                AddField("Reason", i.description, true).
                WithTimestamp(DateTime.UtcNow).
                WithFooter($"INF ID: {i.id}").
                WithColor(BotColors.DARK_ORANGE);

            await ReplyAsync(embed: embed.Build());
        }

        [Command("userinfractions"), Alias("userinf")]
        public async Task UserInfractions(ulong id)
        {


            //await ReplyAsync($"Warned <@{id}> for {i.description}");
        }

        [Command("delinf")]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task DeleteInfraction(ulong id)
        {
            var i = DatabaseService.RemoveInfraction(id);

            await ReplyAsync($"Removed infraction with id {i.id}");
        }

        [Command("editinf"), Alias("modinf")]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task ModifyInfraction(ulong id, params string[] description)
        {
            var i = DatabaseService.GetInfraction(id);

            if (i == null)
            {
                var error = BotUtils.ErrorEmbed(description: "Could not find specified infraction id", withTimestamp: false);
                await ReplyAsync(embed: error.Build());
                return;
            }

            var embed = new EmbedBuilder().
                WithTitle($"Modified Infraction #{i.id}").
                AddField("Previous Reason", i.description).
                AddField("New Reason", string.Join(" ", description)).
                WithTimestamp(DateTime.UtcNow).
                WithFooter($"INF ID: {i.id}").
                WithColor(BotColors.GREEN);

            i.description = string.Join(" ", description);

            DatabaseService.EditInfraction(i);

            await ReplyAsync(embed: embed.Build());


        }
    }
}