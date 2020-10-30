﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using DiscordBot.Core;
using System.Globalization;
using Discord.WebSocket;

namespace DiscordBot.Modules.SingleCommands
{
    public class HelpCommmand: ModuleBase<SocketCommandContext>
    {

        [Command("help")]
        public async Task Help()
        {
            var modules = Assembly.GetEntryAssembly().GetTypes().
                Where(p => p.GetCustomAttribute<HelpModule>() != null).
                ToList();


            var eb = new EmbedBuilder()
            {
                Description = "Here are the modules for Codey! \n" +
                $"Do `{ConfigLoader.Prefix}help <module>` to find out more about a module.",

                Color = new Color(BotColors.DARKER_GREY),

                Timestamp = DateTime.UtcNow
            };

            var user = Context.User as SocketGuildUser;

            foreach(var x in modules)
            {
                var cm = x.GetCustomAttribute<HelpModule>();

                if(user.GuildPermissions.Has(cm.permission)) eb.AddField(cm.name, cm.description);
            }
            await ReplyAsync(embed: eb.Build());
        }

        [Command("help")]
        public async Task HelpModule(params string[] module)
        {
            var modules = Assembly.GetEntryAssembly().GetTypes().
                Where(p => p.GetCustomAttribute<InitializeCommands>() != null).
                GroupBy(p => p.GetCustomAttribute<InitializeCommands>().module);

            var moduleName = string.Join(" ", module);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            var user = Context.User as SocketGuildUser;

            var eb = new EmbedBuilder()
            {
                Title = textInfo.ToTitleCase(moduleName) + " Module",

                Color = new Color(BotColors.GREEN),

                Timestamp = DateTime.UtcNow
            };

            foreach (var x in modules)
            {
                if(x.Key.ToLower().Equals(moduleName, StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach(var y in x)
                    {
                        var methods = y.GetMethods().
                            Where(p => p.GetCustomAttribute<CommandData>() != null);
                        foreach(var m in methods)
                        {
                            var commandData = m.GetCustomAttribute<CommandData>();
                            var reqPermission = m.GetCustomAttribute<RequireUserPermissionAttribute>();
                            var desc = commandData.description;
                            var title = $"{ConfigLoader.Prefix}{commandData.command}";

                            if (reqPermission != null && user.GuildPermissions.Has((GuildPermission)reqPermission?.GuildPermission))
                                eb.AddField(title, desc);
                            else
                                eb.AddField(title, desc);
                        }
                    }
                }
            }

            if(eb.Fields.Count == 0)
            {
                eb = BotUtils.ErrorEmbed("Error Finding Module/Command", $"It looks like this module/command doesn't exist if you believe\n" +
                    $"this is an error contact an adminisrtator");
            }

            await ReplyAsync(embed: eb.Build());
        }

    }
}
