using System;
using Discord.Commands;
using DiscordBot.Core;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using Discord.WebSocket;

namespace DiscordBot.Modules.UserCustomizationModule
{
    [InitializeCommands("User Customization")]
    [HelpModule("User Customization", "Commands that allow the user to customize there appearance.")]
    public class UserCustomization : ModuleBase<SocketCommandContext>
    {
        [Command("giverole")]
        [CommandData("giverole <role>", "Remove a customizable role you have")]
        public async Task GiveRole(string roleName)
        {
            var user = Context.User as SocketGuildUser;
            var roleResults = Context.Guild.Roles.Where(p => roleName.Equals(roleName, StringComparison.OrdinalIgnoreCase)).
                ToList();
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            //Check if the role is an option to remove
            if (roleResults.Count > 0 &&
                (userOptions.userClassifications.Contains(roleResults[0].Id) ||
                userOptions.userTeams.Contains(roleResults[0].Id)))
            {
                var role = Context.Guild.GetRole(roleResults[0].Id);
                await user.AddRoleAsync(role);

                await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Sucessfully added `{roleName}` to user").Build());

                return;
            }

            await ReplyAsync(embed:
                BotUtils.ErrorEmbed(description: $"Could not find `{roleName}` in list of addable roles").Build());
        }

        [Command("removerole")]
        [CommandData("removerole <role>", "Remove a customizable role you have")]
        public async Task RemoveRole(string roleName)
        {
            var user = Context.User as SocketGuildUser;
            var roleResults = user.Roles.Where(p => roleName.Equals(roleName, StringComparison.OrdinalIgnoreCase)).
                ToList();
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            //Check if the role is an option to remove
            if(roleResults.Count > 0 &&
                (userOptions.userClassifications.Contains(roleResults[0].Id) ||
                userOptions.userTeams.Contains(roleResults[0].Id)))
            {
                var role = Context.Guild.GetRole(roleResults[0].Id);
                await user.RemoveRoleAsync(role);

                await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Sucessfully removed `{roleName}` from user").Build());

                return;
            }

            await ReplyAsync(embed:
                BotUtils.ErrorEmbed(description: $"Could not find `{roleName}` attached to user").Build());
        }

        [Command("teams")]
        [CommandData("teams", "Get a list of teams you can give yourself")]
        public async Task Teams()
        {
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            var desc = "";

            foreach (var team in userOptions.userTeams)
            {
                desc += $"\n - {Context.Guild.GetRole(team)?.Name}";
            }

            if (string.IsNullOrWhiteSpace(desc))
            {
                desc = "\n No teams were found...";
            }

            var embed = new EmbedBuilder().
                WithTitle("Teams").
                WithDescription(desc).
                WithColor(new Color((int)BotColors.ORANGE)).
                WithTimestamp(DateTime.UtcNow);

            await ReplyAsync(embed: embed.Build());
        }

        [Command("roles")]
        [CommandData("roles", "Get a list of roles you can give yourself")]
        public async Task Roles()
        {
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            var desc = "";

            foreach(var role in userOptions.userClassifications)
            {
                desc += $"\n - {Context.Guild.GetRole(role)?.Name}";
            }

            if(string.IsNullOrWhiteSpace(desc))
            {
                desc = "\n No roles were found...";
            }

            var embed = new EmbedBuilder().
                WithTitle("Roles").
                WithDescription(desc).
                WithColor(new Color((int)BotColors.ORANGE)).
                WithTimestamp(DateTime.UtcNow);

            await ReplyAsync(embed: embed.Build());
        }
    }
}
