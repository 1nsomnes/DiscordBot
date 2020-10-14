using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;
using Discord;

namespace DiscordBot.Modules.UserCustomizationModule
{
    [InitializeCommands("User Customization")]
    public class AdminUserCustomization : ModuleBase<SocketCommandContext>
    {

        [Command("createrole")]
        [CommandData("createrole <roleId>", "Add a role to the user classifications")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateRole(ulong roleId)
        {
            await ManageRoles(roleId, true, false);
        }

        [Command("destroyrole")]
        [CommandData("destroyrole <roleId>", "Destroy a role in the user classifications")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DestroyRole(ulong roleId)
        {
            await ManageRoles(roleId, false, false);
        }

        [Command("createteam")]
        [CommandData("createteam <roleId>", "Add a role to the teams")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CreateTeam(ulong roleId)
        {
            await ManageRoles(roleId, true, true);
        }

        [Command("destroyteam")]
        [CommandData("destroyteam <roleId>", "Destroy a role in the teams")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DestroyTeam(ulong roleId)
        {
            await ManageRoles(roleId, false, true);
        }

        async Task ManageRoles(ulong id, bool add, bool team)
        {
            var role = Context.Guild.GetRole(id);
            if (role is null)
            {
                await ReplyAsync(embed:
                    BotUtils.ErrorEmbed(description: "Could not find role with specified id",
                    withTimestamp: false).Build());
                return;
            }

            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            if(add)
            {
                //Check if the role is in the user classifications or teams
                if (userOptions.userClassifications.Contains(id) || userOptions.userTeams.Contains(id))
                {
                    await ReplyAsync(embed:
                        BotUtils.ErrorEmbed(description: "Role is already part of the teams or user classifications",
                        withTimestamp: false).Build());
                    return;
                }

                if(team)
                {
                    userOptions.userTeams.Add(id);
                } else
                {
                    userOptions.userClassifications.Add(id);
                }
            }
            else
            {
                //Check if the role is in the user classifications or teams
                if (!userOptions.userClassifications.Contains(id) || !userOptions.userTeams.Contains(id))
                {
                    await ReplyAsync(embed:
                        BotUtils.ErrorEmbed(description: "Role is not a part of teams or user classifications",
                        withTimestamp: false).Build());
                    return;
                }

                if (team)
                {
                    userOptions.userTeams.Remove(id);
                } else
                {
                    userOptions.userClassifications.Add(id);
                }
                
            }

            JSONWrapper w = ConfigLoader.LoadData();
            w.data.userCustomOptions = userOptions;

            ConfigLoader.SaveData(w);

            var roleName = Context.Guild.GetRole(id).Name;

            await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Successfully added `{roleName}` to user classifications",
                    withTimestamp: false).Build());
        }

    }
}
