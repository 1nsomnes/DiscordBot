using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;
using Discord;
using Urban.NET;

namespace DiscordBot.Modules
{
    [InitializeCommands("Fun")]
    [HelpModule("Fun", "Commands that are fun!")]
    public class Fun : ModuleBase<SocketCommandContext>
    {

        [Command("hello")]
        [CommandData("hello")]
        public async Task Hello(params string[] args)
        {
            await Context.Channel.SendMessageAsync("Hello World!");
        }

        [Command("F")]
        [CommandData("F", "F")]
        public async Task F(params string[] args)
        {
            
            var message = await ReplyAsync("F");
            await message.AddReactionAsync(new Emoji("\uD83C\uDDEB"));
        }

        [Command("whenschristmas")]
        [Alias("christams", "christmastime", "timechristmas", "daystillchristmas", "whenchristmas", "snow")]
        [CommandData("whenschristmas", "Self explanatory")]
        public async Task Christmas(params string[] args)
        {
            DateTime today = DateTime.UtcNow;
            DateTime next = new DateTime(today.Year, 12, 25);

            if (next < today) next.AddYears(1);

            var days = (next - today).Days;

            await ReplyAsync($"**{days}** days until Christmas!");
        }

        [Command("urbandictionary"), Alias("ud")]
        [CommandData("urbandictionary <query>", "Search for words on UrbanDictionary")]
        public async Task UrbanDictionary(params string[] query)
        {
            var urbanClient = new UrbanService();
            var data = await urbanClient.Data(string.Join(" ", query));

            if(data.List.Length <= 0)
            {
                var e = BotUtils.ErrorEmbed(description: "Could not find specified word.", withTimestamp: false);
                await ReplyAsync(embed: e.Build());
                return;
            }

            var result = data.List[0];

            var embed = new EmbedBuilder().
                AddField("Definition", result.Definition).
                AddField("Example", result.Example).
                AddField("\nAuthor", result.Author, true).
                AddField("Rating", $":thumbsup:`{result.ThumbsUp}`:thumbsdown:`{result.ThumbsDown}`", true).
                WithFooter($"Result (1/{data.List.Length})").
                WithTimestamp(DateTime.UtcNow).
                WithColor(new Color(BotColors.GREEN)).
                WithThumbnailUrl("https://slack-files2.s3-us-west-2.amazonaws.com/avatars/2018-01-11/297387706245_85899a44216ce1604c93_512.jpg");

            await ReplyAsync(embed: embed.Build());
        }
    }
}
