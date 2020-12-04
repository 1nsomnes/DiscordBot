using System;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Core
{
    public static class BotUtils
    {
        public static EmbedBuilder SuccessEmbed(string title = "", string description = "", bool withTimestamp = true)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color( BotColors.GREEN)
            };

            if (withTimestamp) embed.WithTimestamp(DateTime.UtcNow);
            if (!string.IsNullOrEmpty(title)) embed.Title = title;
            if (!string.IsNullOrEmpty(description)) embed.Description = description;

            return embed;
        }

        public static EmbedBuilder ErrorEmbed(string title = "", string description = "", bool withTimestamp = true)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color( BotColors.RED)
            };

            if (!string.IsNullOrEmpty(title)) embed.Title = title;
            if (!string.IsNullOrEmpty(description)) embed.Description = description;

            return embed;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InitializeCommands : Attribute
    {
        public string module;

        public InitializeCommands(string module)
        {
            this.module = module;
        } 
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class HelpModule : Attribute
    {
        public string name;
        public string description = "other";
        public GuildPermission permission;

        public HelpModule(string name, string description, GuildPermission permission = GuildPermission.SendMessages)
        {
            this.name = name;
            this.description = description;
            this.permission = permission;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandData : Attribute
    {
        public string command;
        public string description;
        public GuildPermission permission;

        public CommandData(string command, string description = "No command description provided.",
            GuildPermission permission = GuildPermission.SendMessages)
        {
            this.command = command;
            this.description = description;
            this.permission = permission;
        }
    }

    public static class Extensions
    {
        public static string ShortenedDate(this DateTimeOffset dto)
        {
            
            return $"{dto.Month}/{dto.Day}/{dto.Year}";
        }

        public static string ShortenedDateTime(this DateTimeOffset dto)
        {

            return $"{dto.Month}/{dto.Day}/{dto.Year}, {dto.Hour}:{dto.Minute} UTC";
        }

        public static string Tag(this IUser u)
        {
            return u.Username + "#" + u.Discriminator;
        }

        public static string TagNickname(this IGuildUser u)
        {
            string tag = $"{u.Username}#{u.Discriminator}";
            if (!string.IsNullOrEmpty(u.Nickname))
            {
                tag += $" ({u.Nickname})";
            }
            return tag;
        }

        public static string FixedAvatarURL(this IUser user)
        {
            return string.IsNullOrEmpty(user.GetAvatarUrl()) ? user.GetDefaultAvatarUrl() : user.GetAvatarUrl();
        }
    }
}
