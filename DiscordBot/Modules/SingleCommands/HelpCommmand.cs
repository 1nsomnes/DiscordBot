using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using DiscordBot.Core;
using System.Globalization;

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

                Color = new Color((int)BotColors.DARKER_GREY),

                Timestamp = DateTime.UtcNow
            };

            foreach(var x in modules)
            {
                var cm = x.GetCustomAttribute<HelpModule>();
                eb.AddField(cm.name, cm.description);
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

            var eb = new EmbedBuilder()
            {
                Title = textInfo.ToTitleCase(moduleName) + " Module",

                Color = new Color((int)BotColors.GREEN),

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
                            var desc = commandData.description;
                            var title = $"{ConfigLoader.Prefix}{commandData.command}";
                            eb.AddField(title, desc);
                        }
                    }
                }
            }

            if(eb.Fields.Count == 0)
            {
                eb = BotUtils.ErrorEmbed("Error Finding Module", $"It looks like this module doesn't exist if you believe\n" +
                    $"this is an error contact an adminisrtator");
            }

            await ReplyAsync(embed: eb.Build());
        }

    }
}
