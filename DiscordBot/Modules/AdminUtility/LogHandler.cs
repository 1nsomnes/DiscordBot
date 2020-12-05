using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBot.Core;
using System.Linq;

namespace DiscordBot.Modules.AdminUtility
{
    [InitializeCommands("Admin Utilities")]
    public class LogHandler : ModuleBase<SocketCommandContext>
    {

        static AdminData adminData;

        public static Task InitializeLogHandler(DiscordSocketClient client)
        {
            adminData = ConfigLoader.LoadData().data.adminData;

            client.MessageUpdated += ClientMessageUpdated;
            client.MessageDeleted += ClientMessageDeleted;
            client.GuildMemberUpdated += GuildMemberUpdated;
            client.UserJoined += JoinedGuild;
            client.UserLeft += LeftGuild;
             

            return Task.CompletedTask;
        }

        private static async Task LeftGuild(SocketGuildUser arg)
        {
            Console.WriteLine("User left");
            if (!adminData.logSettings.userLeft || !adminData.isLogging) return;
            //If the log channel can't be found disable logging
            if (arg.Guild.GetChannel(adminData.logChannelId) == null)
            {
                Console.WriteLine("Error: Could not find channel to log LeftGuild");
                return;
            }

            var joinedDaysAgo = (DateTime.UtcNow - (DateTimeOffset)arg.JoinedAt).Days;
            var dateTime = ((DateTimeOffset)arg.JoinedAt).ShortenedDateTime();

            var embed = new EmbedBuilder().
                WithColor(BotColors.RED).
                WithDescription(arg.Tag() + " left").
                AddField("User Information", arg.Tag() + $" ({arg.Id}) " + arg.Mention).
                AddField("Joined At", $"{dateTime} (**{joinedDaysAgo} days ago**)").
                AddField("ID", $"```swift\nUser = {arg.Id}```").
                WithTimestamp(DateTime.UtcNow).
                WithFooter(new EmbedFooterBuilder().
                    WithIconUrl(Program.bot.client.CurrentUser.FixedAvatarURL()).
                    WithText(Program.bot.client.CurrentUser.Tag()));
                

            embed.Author = new EmbedAuthorBuilder().WithIconUrl(arg.FixedAvatarURL()).WithName(arg.Tag());

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return;

            await msgChannel.SendMessageAsync(embed: embed.Build());
        }

        private static async Task JoinedGuild(SocketGuildUser arg)
        {
            Console.WriteLine("User joined");
            if (!adminData.logSettings.userJoined || !adminData.isLogging) return;
            //If the log channel can't be found disable logging
            if (arg.Guild.GetChannel(adminData.logChannelId) == null)
            {
                Console.WriteLine("Error: Could not find channel to log JoinedGuild");
                return;
            }

            var createdDaysAgo = (DateTime.UtcNow - arg.CreatedAt).Days;

            var embed = new EmbedBuilder().
                WithColor(BotColors.GREEN).
                WithDescription(arg.Tag() + " joined").
                AddField("User Information", arg.Tag() + $" ({arg.Id}) " + arg.Mention).
                AddField("Created At", $"{arg.CreatedAt.ShortenedDateTime()} (**{createdDaysAgo} days ago**)", true).
                AddField("Member Count", arg.Guild.MemberCount, true).
                AddField("ID", $"```swift\nUser = {arg.Id}\nGuild = {arg.Guild.Id}```").
                WithTimestamp(DateTime.UtcNow).
                WithFooter(new EmbedFooterBuilder().
                    WithIconUrl(Program.bot.client.CurrentUser.FixedAvatarURL()).
                    WithText(Program.bot.client.CurrentUser.Tag()));

            embed.Author = new EmbedAuthorBuilder().WithIconUrl(arg.FixedAvatarURL()).WithName(arg.TagNickname());

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return;

            await msgChannel.SendMessageAsync(embed: embed.Build());
        }

        private static async Task GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            if (!adminData.logSettings.nicknameUpdated || !adminData.isLogging) return;
            //If the log channel can't be found disable logging
            if (arg2.Guild.GetChannel(adminData.logChannelId) == null)
            {
                Console.WriteLine("Error: Could not find channel to log GuildMemberUpdated");
                return;
            }

            string oldUsername = string.IsNullOrEmpty(arg1.Nickname) ? arg1.Username : arg1.Nickname;
            string newUsername = string.IsNullOrEmpty(arg2.Nickname) ? arg2.Username : arg2.Nickname;

            if(oldUsername != newUsername)
            {
                var embed = new EmbedBuilder().
                WithColor(BotColors.ORANGE).
                WithDescription(arg2.Tag() + " updated their name").
                AddField("User Information", arg2.Tag() + $" ({arg2.Id}) " + arg2.Mention).
                AddField("Previous Name", $"`{oldUsername}`").
                AddField("New Name", $"`{newUsername}`").
                AddField("ID", $"```swift\nUser = {arg2.Id}```").
                WithTimestamp(DateTime.UtcNow).
                WithFooter(new EmbedFooterBuilder().
                    WithIconUrl(Program.bot.client.CurrentUser.FixedAvatarURL()).
                    WithText(Program.bot.client.CurrentUser.Tag()));

                embed.Author = new EmbedAuthorBuilder().WithIconUrl(arg2.FixedAvatarURL()).WithName(arg2.Tag());

                var channel = Program.bot.client.GetChannel(adminData.logChannelId);
                if (!(channel is ISocketMessageChannel msgChannel)) return;

                await msgChannel.SendMessageAsync(embed: embed.Build());
            }

        }

        private static async Task ClientMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if (!adminData.logSettings.deletedMessages || !adminData.isLogging) return;

            //If the log channel can't be found disable logging
            if((arg2 as SocketGuildChannel).Guild.GetChannel(adminData.logChannelId) == null)
            {
                Console.WriteLine("Error: Could not find channel to log MessageDeleted");
                return;
            }

            if (!arg1.HasValue) return;

            var embed = new EmbedBuilder().
                WithColor(BotColors.RED).
                WithDescription(arg1.Value.Author.Tag() + " deleted a message").
                AddField("User Information", arg1.Value.Author.Tag() + $" ({arg1.Value.Author.Id}) " + arg1.Value.Author.Mention).
                AddField("Channel", $"{arg2.Name} ({arg2.Id}) <#{arg2.Id}>").
                AddField("Content", $"`{arg1.Value.Content}`").
                AddField("ID", $"```swift\nUser = {arg2.Id}\nMessage = {arg1.Value.Id}```").
                WithTimestamp(DateTime.UtcNow).
                WithFooter(new EmbedFooterBuilder().
                    WithIconUrl(Program.bot.client.CurrentUser.FixedAvatarURL()).
                    WithText(Program.bot.client.CurrentUser.Tag()));

            embed.Author = new EmbedAuthorBuilder().WithIconUrl(arg1.Value.Author.FixedAvatarURL()).
                WithName(((IGuildUser)arg1.Value.Author).TagNickname());

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return;

            await msgChannel.SendMessageAsync(embed: embed.Build());
        }

        private static async Task ClientMessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
        {
            if (!adminData.logSettings.editedMessages || !adminData.isLogging) return;
            if (arg2.Author.IsBot || string.IsNullOrEmpty(arg2.Content)) return;

            //If the log channel can't be found disable logging
            if ((arg3 as SocketGuildChannel).Guild.GetChannel(adminData.logChannelId) == null)
            {
                Console.WriteLine("Error: Could not find channel to log MessageUpdated");
                return;
            }

            var prevMsg = arg1.Value?.Content ?? "Could not retrieve contents of message";
            Console.WriteLine("Previous Message: " + prevMsg);

            if (prevMsg.Equals(arg2.Content)) return;

            var embed = new EmbedBuilder().
                WithColor(BotColors.ORANGE).
                WithDescription(arg2.Author.Tag() + $" edited a message\n[Go To Message]({arg2.GetJumpUrl()})").
                AddField("User Information", arg2.Author.Tag() + $" ({arg2.Author.Id}) " + arg2.Author.Mention).
                AddField("Channel Information", $"{arg3.Name} ({arg3.Id}) <#{arg3.Id}>").
                AddField("Previous Content", $"`{prevMsg}`").
                AddField("New Content", $"`{arg2.Content}`").
                AddField("ID", $"```swift\nUser = {arg2.Id}\nMessage = {arg2.Id}```").
                WithTimestamp(DateTime.UtcNow).
                WithFooter(new EmbedFooterBuilder().
                    WithIconUrl(Program.bot.client.CurrentUser.FixedAvatarURL()).
                    WithText(Program.bot.client.CurrentUser.Tag()));

            embed.Author = new EmbedAuthorBuilder().WithIconUrl(arg2.Author.FixedAvatarURL()).
                WithName(((IGuildUser)arg2.Author).TagNickname());

            var channel = Program.bot.client.GetChannel(adminData.logChannelId);
            if (!(channel is ISocketMessageChannel msgChannel)) return; ;

            await msgChannel.SendMessageAsync(embed: embed.Build());
            return;
        }

        /*~~~~~~~~~~~~~~~~~~~~~~~~~
                COMMANDS
        ~~~~~~~~~~~~~~~~~~~~~~~~*/

        [Command("logchannel")]
        [CommandData("logchannel <channel>")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogChannel()
        {
            JSONWrapper w = ConfigLoader.LoadData();

            w.data.adminData.logChannelId = Context.Channel.Id;

            ConfigLoader.SaveData(w);

            var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to <#{Context.Channel.Id}>", withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logchannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogChannel(ISocketMessageChannel channel)
        {
            JSONWrapper w = ConfigLoader.LoadData();

            w.data.adminData.logChannelId = channel.Id;

            ConfigLoader.SaveData(w);

            var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to <#{channel.Id}>", withTimestamp: false).Build();

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

                var embed = BotUtils.SuccessEmbed(description: $"Successfully set log channel to <#{channel.Id}>", withTimestamp: false).Build();

                await ReplyAsync(embed: embed);
            } catch
            {
                var embed = BotUtils.ErrorEmbed("Channel Not Found", "The channel id provided was not found").Build();

                await ReplyAsync(embed: embed);
            }
        }

        [Command("logging")]
        [CommandData("logging <true/false>")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Logging(bool val)
        {
            if(Context.Guild.GetChannel(adminData.logChannelId) == null)
            {
                var e = BotUtils.ErrorEmbed(description: $"You cannot run this command because the logging channel is not set. \n" +
                    $"Try running `>logchannel <channel>` to set the logchannel.",
                withTimestamp: false).Build();

                await ReplyAsync(embed: e);

                return;
            }
            adminData.isLogging = val;

            var newConfig = ConfigLoader.LoadData();
            newConfig.data.adminData = adminData;

            ConfigLoader.SaveData(newConfig);

            var embed = BotUtils.SuccessEmbed(description: $"Logging `{(val ? "enabled" : "disabled")}`",
                withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logging")]
        [CommandData("logging", "Tells you if logging is enabled or not")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Logging(params string[] args)
        {
            if (args.Length > 0 && args[0].Equals("enable", StringComparison.CurrentCultureIgnoreCase)) { await Logging(true); return; }
            if (args.Length > 0 && args[0].Equals("disable", StringComparison.CurrentCultureIgnoreCase)) { await Logging(false); return; }
            var embed = BotUtils.SuccessEmbed(description: $"Logging is currently `{(adminData.isLogging ? "enabled" : "disabled")}`",
                withTimestamp: false).Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logsettings")]
        [CommandData("logsettings <setting> <true/false>" , "Turn on or off a log setting")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogSettings()
        {
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //       USE GET FIELDS
            // TO GET PUBLIC PROPERTIES
            //       USE GET PROPS
            // TO GET PRIVATE PROPERTIES
            //          ~~~
            //  BINDING FLAGS TO SPECIFY
            //       PROPERTY INFO
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~
            var settings = adminData.logSettings.GetType().
                GetFields().Where(p => p.FieldType == typeof(bool)).ToList();

            var desc = "";

            foreach(var setting in settings)
            { 
                //                   When using FieldInfo.GetValue(instance) you must specify the instance you are getting it from.
                desc += $"\n {setting.Name} = `{setting.GetValue(adminData.logSettings).ToString().ToLower()}`";
            }

            if (!adminData.isLogging) desc += "\n\n **!WARNING! Logging is disabled**";

            var embed = new EmbedBuilder().
                WithTitle("Log Settings").
                WithDescription(desc).
                WithTimestamp(DateTime.UtcNow).
                WithColor( BotColors.ORANGE).
                Build();

            await ReplyAsync(embed: embed);
        }

        [Command("logsettings")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogSettings(string setting, bool val)
        {
            var settings = adminData.logSettings.GetType().
                GetFields().Where(p => p.Name.Equals(setting, StringComparison.OrdinalIgnoreCase))
                .ToList();

            try
            {
                settings[0].SetValue(adminData.logSettings, val);

                JSONWrapper w = ConfigLoader.LoadData();
                w.data.adminData.logSettings = adminData.logSettings;
                ConfigLoader.SaveData(w);

                var embed = BotUtils.SuccessEmbed(description: $"Logging `{settings[0].Name}` set to `{(val ? "true" : "false")}`",
                withTimestamp: false);

                if (!adminData.isLogging) embed.WithDescription(embed.Description + "\n\n **!WARNING! Logging is disabled**");

                await ReplyAsync(embed: embed.Build());
            }
            catch
            {
                var embed = BotUtils.ErrorEmbed(description: "Error finding setting", withTimestamp: false).Build();

                await ReplyAsync(embed: embed);
            }
        }
    }
}
