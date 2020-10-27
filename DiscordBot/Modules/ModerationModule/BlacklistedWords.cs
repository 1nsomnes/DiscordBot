using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBot.Core;
using Discord;

namespace DiscordBot.Modules.ModerationModule
{
    [InitializeCommands("Moderation")]
    public class BlacklistedWords : ModuleBase<SocketCommandContext>
    {

        [Command("addblacklist")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task AddBlacklist(params string[] args)
        {
            //if the blacklisted words already contains the specified value throw an error
            var blacklistedWords = ConfigLoader.LoadData().data.moderationSettings.blacklistedWords;
            var embed = new EmbedBuilder();

            if (blacklistedWords.Contains(string.Join(" ", args)))
            {
                embed = BotUtils.ErrorEmbed(description: $"Blacklist already contains " +
                    $"`{string.Join(" ", args)}`", withTimestamp: false);
            }
            else
            {
                blacklistedWords.Add(string.Join(" ", args));
                var w = ConfigLoader.LoadData();
                w.data.moderationSettings.blacklistedWords = blacklistedWords;
                ConfigLoader.SaveData(w);

                embed = BotUtils.SuccessEmbed(description: $"`{string.Join(" ", args)}` succesfully added to blacklist",
                    withTimestamp: false);
            }

            await ReplyAsync(embed: embed.Build());
        }

        [Command("delblacklist")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task DelBlacklist(params string[] args)
        {
            //if the blacklisted words does not contain specifed value throw an error
            var blacklistedWords = ConfigLoader.LoadData().data.moderationSettings.blacklistedWords;
            var embed = new EmbedBuilder();

            if (!blacklistedWords.Contains(string.Join(" ", args)))
            {
                embed = BotUtils.ErrorEmbed(description: $"Blacklist does not contain {string.Join(" ", args)}", withTimestamp: false);
                await ReplyAsync(embed: embed.Build());
                return; 
            }

            blacklistedWords.Remove(string.Join(" ", args));
            var w = ConfigLoader.LoadData();
            w.data.moderationSettings.blacklistedWords = blacklistedWords;
            ConfigLoader.SaveData(w);

            embed = BotUtils.SuccessEmbed(description: $"`{string.Join(" ", args)}` succesfully removed from blacklist",
                    withTimestamp: false);

            await ReplyAsync(embed: embed.Build());

        }

        [Command("blacklist")]
        public async Task Blacklist()
        {
            var blacklistedWords = ConfigLoader.LoadData().data.moderationSettings.blacklistedWords;

            var description = blacklistedWords.Count > 0 ? string.Join(", ", blacklistedWords) : "No words found in blacklist";
            var embed = BotUtils.ErrorEmbed("Blacklisted Words Are: ", description, false);

            await ReplyAsync(embed: embed.Build());
        }
    }
}
