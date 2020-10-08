using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBot.Core;

namespace DiscordBot.Modules.AdminUtility
{
    public class LogHandler : ModuleBase<SocketCommandContext>
    {

        private static DiscordSocketClient Client;
        static AdminData adminData;

        public static Task InitializeLogHandler(DiscordSocketClient client)
        {
            adminData = ConfigLoader.LoadData().data.adminData;
            Client = client;

            client.MessageUpdated += ClientMessageUpdated;
            client.MessageDeleted += ClientMessageDeleted;

            return Task.CompletedTask;
        }

        private static async Task ClientMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if (adminData.logChannelId == 0 || !adminData.isLogging) return;

            var embed = arg1.HasValue ? BotUtils.ErrorEmbed($"{arg1.Value.Author.Username}#{arg1.Value.Author.Discriminator} Deleted",
                $"<#{arg1.Value.Channel.Id}>: `{arg1.Value.Content}`").WithFooter($"ID: {arg1.Value.Author.Id}").
                WithColor(new Color((int)BotColors.DARK_ORANGE))
                : BotUtils.ErrorEmbed(description: "Message was deleted but the information was lost");

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return;

            await msgChannel.SendMessageAsync(embed: embed.Build());
            return;
        }

        private static async Task ClientMessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            if (adminData.logChannelId == 0 || !adminData.isLogging) return;
            if (arg2.Author.IsBot || string.IsNullOrEmpty(arg2.Content)) return;

            var prevMsg = arg1.Value?.Content ?? "Could not retrieve contents of message";  

            var embed = BotUtils.SuccessEmbed($"{arg2.Author.Username}#{arg2.Author.Discriminator}" + " Editted");

            embed.WithDescription($"In <#{arg2.Channel.Id}>");
            embed.WithFooter($"ID: " + arg2.Id);
            embed.AddField("Before", $"`{prevMsg}`");
            embed.AddField("After", $"`{arg2.Content}`");
            embed.Color = new Color((int)BotColors.ORANGE);

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return; ;

            await msgChannel.SendMessageAsync(embed: embed.Build());
            return;
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogChannel()
        {
            JSONWrapper w = ConfigLoader.LoadData();

            w.data.adminData.logChannelId = Context.Channel.Id;

            ConfigLoader.SaveData(w);

            var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to **#{Context.Channel}**", withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogChannel(ISocketMessageChannel channel)
        {
            JSONWrapper w = ConfigLoader.LoadData();

            w.data.adminData.logChannelId = channel.Id;

            ConfigLoader.SaveData(w);

            var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to **#{channel}**", withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogChannel(ulong channelId)
        {
            try
            {
                var channel = Context.Guild.GetChannel(channelId);

                JSONWrapper w = ConfigLoader.LoadData();

                w.data.adminData.logChannelId = channel.Id;

                ConfigLoader.SaveData(w);

                var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to #{channel}", withTimestamp: false).Build();

                await ReplyAsync(embed: embed);
            } catch
            {
                var embed = BotUtils.ErrorEmbed("Channel Not Found", "The channel id provided was not found").Build();

                await ReplyAsync(embed: embed);
            }
        }

        [Command("logging")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Logging(bool val)
        {
            adminData.isLogging = val;

            var newConfig = ConfigLoader.LoadData();
            newConfig.data.adminData = adminData;

            ConfigLoader.SaveData(newConfig);

            var embed = BotUtils.SuccessEmbed(description: $"Logging `{(val ? "enabled" : "disabled")}`",
                withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logging")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Logging(params string[] args)
        {
            if (args[0].Equals("enable", StringComparison.CurrentCultureIgnoreCase)) { await Logging(true); return; }
            if (args[0].Equals("disable", StringComparison.CurrentCultureIgnoreCase)) { await Logging(false); return; }
            var embed = BotUtils.SuccessEmbed(description: $"Logging is `{(adminData.isLogging ? "enabled" : "disabled")}`",
                withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }
    }
}
