using Discord.Interactions;
using tsgsBot_C_.Utils;
using Discord;

namespace tsgsBot_C_.Commands.Public
{
    public sealed class RemindCommand : LoggedCommandModule
    {
        [SlashCommand("remind", "Set a reminder for a task at a specific time")]
        [DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
        public async Task RemindAsync(
            [Summary("task", "The task or item to remind you about")] string task,
            [Summary("duration", "When to remind you (e.g. 7h 30m, 2d, 1w)")] string duration)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("task", task), ("duration", duration));

            try
            {
                TimeSpan? delay = HelperMethods.ParseDuration(duration);

                if (delay is null || delay.Value <= TimeSpan.Zero)
                {
                    await FollowupAsync(
                        "❌ I couldn't understand that time.\n" +
                        "Try something like `10m`, `1h 30m`, or `2d`.",
                        ephemeral: true);
                    return;
                }

                DateTimeOffset reminderTime = DateTimeOffset.UtcNow.Add(delay.Value);

                // Human-readable delay
                List<string> parts = new();

                if (delay.Value.Days > 0)
                    parts.Add($"{delay.Value.Days} day{(delay.Value.Days > 1 ? "s" : "")}");
                if (delay.Value.Hours > 0)
                    parts.Add($"{delay.Value.Hours} hour{(delay.Value.Hours > 1 ? "s" : "")}");
                if (delay.Value.Minutes > 0)
                    parts.Add($"{delay.Value.Minutes} minute{(delay.Value.Minutes > 1 ? "s" : "")}");
                if (delay.Value.Seconds > 0 || parts.Count == 0)
                    parts.Add($"{delay.Value.Seconds} second{(delay.Value.Seconds > 1 ? "s" : "")}");

                string delayText = string.Join(", ", parts);

                await FollowupAsync(
                    $"Reminder set for **{task}** at <t:{reminderTime.ToUnixTimeSeconds()}:F>.\n" +
                    $"I will remind you in {delayText}.",
                    ephemeral: true);

                // Fire reminder after delay (non-persistent!) TODO, ADD Persistence to DB like Polls and Giveaways
                _ = Task.Delay(delay.Value).ContinueWith(async _ =>
                {
                    try
                    {
                        await Context.User.SendMessageAsync($"🔔 Reminder: **{task}**");
                    }
                    catch (Exception ex)
                    {
                        // User might have DMs closed → log or ignore
                        Console.WriteLine($"Failed to send reminder DM: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                await FollowupAsync($"Error setting reminder: {ex.Message}", ephemeral: true);
            }
        }
    }
}