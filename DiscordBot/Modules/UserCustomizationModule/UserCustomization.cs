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
        [Command("giverole"),Alias("addrole")]
        [CommandData("giverole <role>", "Remove a customizable role you have")]
        public async Task GiveRole(params string[] roleNameArgs)
        {
            var roleName = string.Join(" ", roleNameArgs);
            var user = Context.User as SocketGuildUser;
            var roleResults = Context.Guild.Roles.Where(p => roleName.Equals(p.Name, StringComparison.OrdinalIgnoreCase)).
                ToList();
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            Console.WriteLine(roleResults[0].Id);

            //Check if the role is an option to remove
            if (roleResults.Count > 0 &&
                (userOptions.userClassifications.Contains(roleResults[0].Id) ||
                userOptions.userTeams.Contains(roleResults[0].Id)))
            {
                var role = Context.Guild.GetRole(roleResults[0].Id);
                //Check if player alread has the role
                if(user.Roles.Where(p => p.Id == role.Id).ToList().Count > 0)
                {
                    await ReplyAsync(embed:
                        BotUtils.ErrorEmbed(description: $"You already have this role. If you want to remove a role do " +
                        $"`{ConfigLoader.Prefix}removerole <role>`",
                        withTimestamp: false).Build());
                    return;
                }

                await user.AddRoleAsync(role);

                //Check if this is a team role so that
                //The bot can remove other team roles so there are no conflicts.
                if(userOptions.userTeams.Contains(role.Id))
                {
                    foreach(var teamId in userOptions.userTeams)
                    {
                        var teamRole = Context.Guild.GetRole(teamId);
                        //If it is not the role being added and the user has it; remove it
                        if (teamId != role.Id && user.Roles.Where(p => p.Id == teamId).ToList().Count > 0)
                            await user.RemoveRoleAsync(teamRole);
                    }
                }

                await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Sucessfully added `{roleName}` to user",
                    withTimestamp: false).Build());

                return;
            }

            await ReplyAsync(embed:
                BotUtils.ErrorEmbed(description: $"Could not find `{roleName}` in list of addable roles",
                withTimestamp: false).Build());
        }

        [Command("removerole")]
        [CommandData("removerole <role>", "Remove a customizable role you have")]
        public async Task RemoveRole(params string[] roleNameArgs)
        {
            var roleName = string.Join(" ", roleNameArgs);
            var user = Context.User as SocketGuildUser;
            var roleResults = user.Roles.Where(p => roleName.Equals(p.Name, StringComparison.OrdinalIgnoreCase)).
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
                    BotUtils.SuccessEmbed(description: $"Sucessfully removed `{roleName}` from user",
                    withTimestamp: false).Build());

                return;
            }

            await ReplyAsync(embed:
                BotUtils.ErrorEmbed(description: $"Could not find `{roleName}` attached to user",
                withTimestamp: false).Build());
        }

        [Command("teams")]
        [CommandData("teams", "Get a list of teams you can give yourself")]
        public async Task Teams()
        {
            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            var desc = $"Do `{ConfigLoader.Prefix}giverole <teamName>` to join a team";

            foreach (var team in userOptions.userTeams)
            {
                desc += $"\n  -{Context.Guild.GetRole(team)?.Name}";
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

            var desc = $"Do `{ConfigLoader.Prefix}giverole <roleName>` to join a role";

            foreach (var role in userOptions.userClassifications)
            {
                desc += $"\n  -{Context.Guild.GetRole(role)?.Name}";
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
