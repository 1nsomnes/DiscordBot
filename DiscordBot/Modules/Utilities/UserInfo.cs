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
        [CommandData("userinfo <user>", "Get information about a user in the discord server")]
        public async Task UserInfoCommand()
        {
            IGuildUser author = Context.Guild.GetUser(Context.Message.Author.Id);
            var embed = new EmbedBuilder()
            {
                Title = $"User info on {author.Username}:",

                ThumbnailUrl = author.GetAvatarUrl(),

                Color = new Color((int)BotColors.GREEN),

                Timestamp = DateTime.UtcNow
            };

            var joinedAt = (DateTimeOffset)author.JoinedAt;

            embed.AddField("Username", author.Tag(), true);
            embed.AddField("ID", author.Id, true);
            embed.AddField("Join Date", joinedAt.ShortenedDate(), true);

            embed.AddField("\nCreation Date", author.CreatedAt.ShortenedDate(), true);

            await ReplyAsync(embed: embed.Build());
        }

        [Command("userinfo")]
        [DefaultCommandInitializer]
        public async Task UserInfoCommand(IGuildUser author)
        {
            var embed = new EmbedBuilder()
            {
                Title = $"User info on {author.Username}:",

                ThumbnailUrl = author.GetAvatarUrl(),

                Color = new Color((int)BotColors.GREEN),

                Timestamp = DateTime.UtcNow
            };

            var joinedAt = (DateTimeOffset)author.JoinedAt;

            embed.AddField("Username", author.Tag(), true);
            embed.AddField("ID", author.Id, true);
            embed.AddField("Join Date", joinedAt.ShortenedDate(), true);

            embed.AddField("\nCreation Date", author.CreatedAt.ShortenedDate(), true);

            await ReplyAsync(embed: embed.Build());
        }

        [Command("userinfo")]
        public async Task UserInfoCommand(ulong userId)
        {
            try
            {
                await ReplyAsync(embed: userInfoEmbed(Context.Guild.GetUser(userId)).Build());
            }
            catch
            {
                var embed = BotUtils.ErrorEmbed("Error Searching for User", "Remember this user must be inside the guild. Otherwise \n" +
                    "the command will not work.");
                await ReplyAsync(embed: embed.Build());
            }
        }

        private EmbedBuilder userInfoEmbed(IGuildUser author)
        {
            var embed = new EmbedBuilder()
            {
                Title = $"User info on {author.Username}:",

                ThumbnailUrl = author.GetAvatarUrl(),

                Color = new Color((int)BotColors.GREEN),

                Timestamp = DateTime.UtcNow
            };

            var joinedAt = (DateTimeOffset)author.JoinedAt;

            embed.AddField("Username", author.Tag(), true);
            embed.AddField("ID", author.Id, true);
            embed.AddField("Join Date", joinedAt.ShortenedDate(), true);

            embed.AddField("\nCreation Date", author.CreatedAt.ShortenedDate(), true);

            return embed;
        }
    }
}
