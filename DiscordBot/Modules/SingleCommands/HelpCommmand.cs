using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using DiscordBot.Core;

namespace DiscordBot.Modules.SingleCommands
{
    public class HelpCommmand: ModuleBase<SocketCommandContext>
    {

        [Command("help")]
        public async Task Help()
        {
            var modules = Assembly.GetEntryAssembly().GetTypes().
                Where(p => p.GetCustomAttribute<CustomModule>() != null).
                ToList();

            var eb = new EmbedBuilder()
            {
                Description = "Here are the modules for Codey! \n" +
                $"Do `{ConfigLoader.Prefix}help <module>` to find out more about a module.",

                Timestamp = DateTime.UtcNow
            };

            foreach(var x in modules)
            {
                CustomModule cm = x.GetCustomAttribute<CustomModule>();
                eb.AddField(cm.name, cm.description);
            }
            await ReplyAsync(embed: eb.Build());
        }

        [Command("help")]
        public async Task HelpModule(params string[] module)
        {
            var modules = Assembly.GetEntryAssembly().GetTypes().
                Where(p => p.GetCustomAttribute<CustomModule>() != null).
                GroupBy(p => p.GetCustomAttribute<CustomModule>().name);

            var moduleName = string.Join(" ", module);

            var eb = new EmbedBuilder()
            {
                Title = moduleName + " Module",

                Color = new Color(0, 255, 0)
            };

            foreach (var x in modules)
            {
                if(x.Key == moduleName)
                {
                    foreach(var y in x)
                    {
                        var methods = y.GetMethods().
                            Where(p => p.GetCustomAttribute<CommandAttribute>() != null);
                        foreach(var m in methods)
                        {
                            var desc = m.GetCustomAttribute<Core.ModuleDescription>()?.description ??
                                "No description was provided for this command.";
                            var title = $"{ConfigLoader.Prefix}{m.GetCustomAttribute<CommandAttribute>().Text}";
                            eb.AddField(title, desc);
                        }
                    }
                }
            }

            if(eb.Fields.Count == 0)
            {
                eb = new EmbedBuilder()
                {
                    Title = "Error Finding Module",

                    Description = $"It looks like this module doesn't exist if you believe\n" +
                    $"this is an error contact an adminisrtator",

                    Timestamp = DateTime.UtcNow,

                    Color = new Color(255, 0, 0)
                };
            }

            await ReplyAsync(embed: eb.Build());
        }

    }
}
