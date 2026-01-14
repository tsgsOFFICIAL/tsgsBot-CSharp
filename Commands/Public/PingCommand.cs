using Discord.Interactions;
using System.Diagnostics;
using Discord;

namespace tsgsBot_C_.Commands.Public
{
    public sealed class PingCommand : LoggedCommandModule
    {
        [SlashCommand("ping", "Check the bot's latency.")]
        [CommandContextType(InteractionContextType.Guild | InteractionContextType.BotDm)]
        [DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
        public async Task PingAsync()
        {
            await LogCommandAsync();
            await DeferAsync();

            // Bot -> Discord latency
            int discordLatency = Context.Client.Latency;

            // Measure Bot -> User latency
            Stopwatch stopwatch = Stopwatch.StartNew();
            await RespondAsync("Calculating...", ephemeral: true);
            stopwatch.Stop();

            long userLatency = stopwatch.ElapsedMilliseconds;

            // Update message with full latency info
            await ModifyOriginalResponseAsync(msg =>
            {
                msg.Content = $"🏓 Pong!\n" +
                              $"**Latency (Bot → Discord):** {discordLatency} ms\n" +
                              $"**Latency (Bot → You):** {userLatency} ms";
            });
        }
    }
}
