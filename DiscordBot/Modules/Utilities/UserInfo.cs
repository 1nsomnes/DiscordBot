using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules.Utilities
{
    [InitializeCommands("Utilities")]
    public class UserInfo : ModuleBase<SocketCommandContext>
    {
        [Command("userinfo")]
        public async Task UserInfoCommand()
        {
            
        }

        public async Task UserInfoCommand(IGuildUser user)
        {

        }

        public async Task UserInfoCommand(int userId)
        {

        }
    }
}
