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
        [Alias("h")]
        public async Task Help()
        {
            var modules = Assembly.GetEntryAssembly().GetTypes().
                Where(p => p.GetCustomAttribute<CustomModule>() != null).
                ToList();

            var eb = new EmbedBuilder()
            {
                Description = "Here are the modules for this bot! \n" +
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

    }
}
