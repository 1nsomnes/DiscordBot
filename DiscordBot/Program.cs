using System;
using DiscordBot.Core;
using System.Threading;

namespace DiscordBot
{
    class MainClass
    {
        public static Bot bot = new Bot();

        public static void Main(string[] args) => bot.RunBot().GetAwaiter().GetResult();
    }
}
