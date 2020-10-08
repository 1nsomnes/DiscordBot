using System;
using Discord;

namespace DiscordBot.Core
{
    public static class BotUtils
    {
        public static EmbedBuilder SuccessEmbed(string title = "", string description = "", bool withTimestamp = true)
        {
            var embed = new EmbedBuilder()
            {
                Color = new Color((int)BotColors.GREEN)
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
                Color = new Color((int)BotColors.RED)
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

        public HelpModule(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandData : Attribute
    {
        public string command;
        public string description;

        public CommandData(string command, string description = "No command description provided.")
        {
            this.command = command;
            this.description = description;

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

            return $"{dto.Month}/{dto.Day}/{dto.Year} {dto.Hour}:{dto.Minute}{dto.TimeOfDay}";
        }
    }
}
