using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;
using DiscordBot;
using Discord;
using System.Linq;

namespace DiscordBot.Modules.Utilities
{
    [InitializeCommands("Utilities")]
    [HelpModule("Utilities", "Quality of life commands that will inhance your discord experience.")]
    public class UtilityCommands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [CommandData("ping" , "Get latency of the bot")]
        public async Task Ping(params string[] args)
        {
            await Context.Channel.SendMessageAsync($"Pong **{Program.bot.client.Latency}ms**");
        }

        [Command("serverinfo"), Alias("guildinfo")]
        [CommandData("serverinfo", "Get info about the server")]
        public async Task ServerInfo(params string[] args)
        {
            var eb = new EmbedBuilder()
            {
                Title = Context.Guild.Name,

                Description = Context.Guild.Description,

                Timestamp = DateTime.UtcNow,

                Color = new Color(BotColors.BLUE)
            };

            eb.AddField("Owner", Context.Guild.Owner, true);
            eb.AddField("Server Creation", Context.Guild.CreatedAt.ShortenedDate(), true);
            eb.AddField("Member Count", Context.Guild.MemberCount, true);
            eb.WithFooter($"ID: {Context.Guild.Id}");

            string roles = "";
            foreach (var x in Context.Guild.Roles) roles += x.Mention;
            eb.AddField("Roles", roles, false);
            
            await ReplyAsync(embed: eb.Build());
        }

        [Command("github")]
        [CommandData("github", "GitHub repo link for this bot")]
        public async Task GitHubLink(params string[] args)
        {
            var embed = new EmbedBuilder()
            {
                Title = "Github",

                Description = "Codey bot is an opensource bot you can \n" +
                "find the public repository here: \n\n" +
                "[GitHub Repo](https://github.com/1nsomnes/DiscordBot)",

                Timestamp = DateTime.UtcNow,

                Color = new Color(BotColors.GREEN)
            }.Build();

            await ReplyAsync(embed: embed);
        }

    }
}
