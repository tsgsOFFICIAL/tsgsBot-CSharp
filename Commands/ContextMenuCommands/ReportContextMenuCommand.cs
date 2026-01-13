using Discord.Interactions;
using Discord.WebSocket;
using Discord;

namespace tsgsBot_C_.Commands.ContextMenuCommands
{
    public sealed class ReportContextMenuCommand : LoggedCommandModule
    {
        [MessageCommand("Report Message")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
        public async Task ReportMessageAsync(IMessage message)
        {
            await DeferAsync(ephemeral: true);

            await LogCommandAsync(("messageId", message.Id), ("messageAuthorId", message.Author.Id));

            SocketUser reporter = Context.User;
            SocketTextChannel? ReportsChannel = Context.Guild.TextChannels.FirstOrDefault(channel => channel.Name == "reports");

            if (ReportsChannel == null)
            {
                await FollowupAsync("⚠️ Couldn't find the reports channel.", ephemeral: true);
                return;
            }

            EmbedBuilder embed = new EmbedBuilder()
                .WithTitle("🚨 Message Report")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.UtcNow)
                .AddField("Reporter", reporter.Mention, inline: true)
                .AddField("Author", message.Author.Mention, inline: true)
                .AddField(
                    "Channel",
                    Context.Channel is IGuildChannel guildChannel
                        ? $"<#{guildChannel.Id}>"
                        : "Unknown",
                    inline: true)
                .AddField("Message Content", string.IsNullOrWhiteSpace(message.Content) ? "*No content*" : message.Content, inline: false)
                .AddField("Message Link", $"[Jump to message]({message.GetJumpUrl()})", inline: false);

            if (message.Attachments.Any())
            {
                embed.AddField("Attachments", string.Join("\n", message.Attachments.Select(a => a.Url)), inline: false);
            }

            await ReportsChannel.SendMessageAsync(embed: embed.Build());

            await FollowupAsync("✅ The message has been reported to the moderators.", ephemeral: true);
        }
    }
}