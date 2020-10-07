using System;
namespace DiscordBot.Core
{
    public class BotUtils
    {
        
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
}
