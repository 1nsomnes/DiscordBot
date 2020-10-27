using System;
using Discord.Commands;
using DiscordBot.Core;
using Discord;

namespace DiscordBot.Modules.ModerationModule
{
    [HelpModule("Moderation", "Commands for moderators", GuildPermission.ManageMessages)]
    public class ModerationMain : ModuleBase<SocketCommandContext>
    {
        
    }
}
