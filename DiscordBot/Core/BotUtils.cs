using System;
using Discord;

namespace DiscordBot.Core
{
    public static class BotUtils
    {
        public static EmbedBuilder ErrorEmbed(string title, string description)
        {
            return new EmbedBuilder()
            {
                Title = title,

                Description = description,

                Timestamp = DateTime.UtcNow,

                Color = new Color(255, 0, 0)
            };
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomModule : Attribute
    {
        public string name;
        public string description = "other";

        public CustomModule(string name, string description)
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
}
