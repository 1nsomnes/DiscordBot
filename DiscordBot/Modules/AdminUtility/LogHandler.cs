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

            return Task.CompletedTask;
        }

        private static async Task GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            if (!adminData.logSettings.nicknameUpdated || !adminData.isLogging) return;
            //If the log channel can't be found disable logging
            if (arg2.Guild.GetChannel(adminData.logChannelId) == null)
            {
                adminData.isLogging = false;
                return;
            }

            //When arg1.Nickname is empty it threw an error when doing arg1.Nickname.Equals
            //This solves that
            var tempNickname = arg1.Nickname;
            if (string.IsNullOrEmpty(arg1.Nickname))
                tempNickname = "stuff";

            if (!tempNickname.Equals(arg2.Nickname))
            {
                var user = arg2 as IUser;
                var firstNickname = string.IsNullOrEmpty(arg1.Nickname) ? arg1.Username : arg1.Nickname;
                var secondNickname = string.IsNullOrEmpty(arg2.Nickname) ? arg2.Username : arg2.Nickname;
                var embed = new EmbedBuilder().
                    WithTitle($"{user.Tag()} Nickname Updated").
                    AddField("Before", $"`{firstNickname}`").
                    AddField("After", $"`{secondNickname}`").
                    WithTimestamp(DateTime.UtcNow).
                    WithFooter($"ID: {arg1.Id}").
                    WithColor(new Color((int)BotColors.ORANGE)).
                    Build();

                var channel = Program.bot.client.GetChannel(adminData.logChannelId);
                if (!(channel is ISocketMessageChannel msgChannel)) return;

                await msgChannel.SendMessageAsync(embed: embed);
            }
        }

        private static async Task ClientMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            if (!adminData.logSettings.deletedMessages || !adminData.isLogging) return;

            //If the log channel can't be found disable logging
            if((arg2 as SocketGuildChannel).Guild.GetChannel(adminData.logChannelId) == null)
            {
                adminData.isLogging = false;
                return;
            }

            var embed = arg1.HasValue ? BotUtils.ErrorEmbed($"{arg1.Value.Author.Tag()} Deleted",
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
            if (!adminData.logSettings.editedMessages || !adminData.isLogging) return;
            if (arg2.Author.IsBot || string.IsNullOrEmpty(arg2.Content)) return;

            //If the log channel can't be found disable logging
            if ((arg3 as SocketGuildChannel).Guild.GetChannel(adminData.logChannelId) == null)
            {
                adminData.isLogging = false;
                return;
            }

            var prevMsg = arg1.Value?.Content ?? "Could not retrieve contents of message";  

            var embed = BotUtils.SuccessEmbed($"{arg2.Author.Tag()} Editted");

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
                WithColor((int)BotColors.ORANGE).
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
