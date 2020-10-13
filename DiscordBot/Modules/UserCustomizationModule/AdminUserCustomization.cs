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
            var role = Context.Guild.GetRole(roleId);
            if(role is null)
            {
                await ReplyAsync(embed:
                    BotUtils.ErrorEmbed(description: "Could not find role with specified id",
                    withTimestamp: false).Build());
                return;
            }

            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            if(userOptions.userClassifications.Contains(roleId))
            {
                await ReplyAsync(embed:
                    BotUtils.ErrorEmbed(description: "Role is already part of user classifications",
                    withTimestamp: false).Build());
                return;
            }

            userOptions.userClassifications.Add(roleId);

            JSONWrapper w = ConfigLoader.LoadData();
            w.data.userCustomOptions = userOptions;

            ConfigLoader.SaveData(w);

            var roleName = Context.Guild.GetRole(roleId).Name;

            await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Successfully added `{roleName}` to user classifications",
                    withTimestamp: false).Build());
        }

        [Command("destroyrole")]
        [CommandData("destroyrole <roleId>", "Destroy a role in the user classifications")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DestroyRole(ulong roleId)
        {
            var role = Context.Guild.GetRole(roleId);
            if (role is null)
            {
                await ReplyAsync(embed:
                    BotUtils.ErrorEmbed(description: "Could not find role with specified id",
                    withTimestamp: false).Build());
                return;
            }

            var userOptions = ConfigLoader.LoadData().data.userCustomOptions;

            if (!userOptions.userClassifications.Contains(roleId))
            {
                await ReplyAsync(embed:
                    BotUtils.ErrorEmbed(description: "Role is not a part of user classifications",
                    withTimestamp: false).Build());
                return;
            }

            userOptions.userClassifications.Remove(roleId);

            JSONWrapper w = ConfigLoader.LoadData();
            w.data.userCustomOptions = userOptions;

            ConfigLoader.SaveData(w);

            var roleName = Context.Guild.GetRole(roleId).Name;

            await ReplyAsync(embed:
                    BotUtils.SuccessEmbed(description: $"Successfully removed `{roleName}` to user classifications",
                    withTimestamp: false).Build());
        }
    }
}
