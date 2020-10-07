using System;
namespace DiscordBot.Core
{
    public class BotUtils
    {
        
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HelpOption : Attribute
    {
        public string name;
        public string category = "other";
    }
}
