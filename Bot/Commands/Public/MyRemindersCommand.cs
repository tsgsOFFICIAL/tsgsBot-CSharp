using Discord.Interactions;
using tsgsBot_C_.Services;
using tsgsBot_C_.Models;
using tsgsBot_C_.Utils;
using Discord;

namespace tsgsBot_C_.Bot.Commands.Public
{
    public sealed class MyRemindersCommand : LoggedCommandModule
    {
        [SlashCommand("myreminders", "View all your active reminders")]
        [DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
        public async Task MyRemindersAsync()
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync();

            try
            {
                List<DatabaseReminderModel> reminders = await DatabaseService.Instance.GetUserRemindersAsync(Context.User.Id);

                if (reminders.Count == 0)
                {
                    await FollowupAsync(
                        "📭 You don't have any reminders.",
                        ephemeral: true);
                    return;
                }

                // Filter to only active (not sent) reminders
                List<DatabaseReminderModel> activeReminders = reminders.Where(r => !r.HasSent).ToList();

                if (activeReminders.Count == 0)
                {
                    await FollowupAsync(
                        "📭 You don't have any active reminders.",
                        ephemeral: true);
                    return;
                }

                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"📋 Your Reminders ({activeReminders.Count})")
                    .WithColor(Color.Blue)
                    .WithFooter($"Requested by {Context.User}")
                    .WithTimestamp(DateTime.UtcNow);

                foreach (DatabaseReminderModel? reminder in activeReminders.OrderBy(r => r.ReminderTime))
                {
                    TimeSpan timeUntilReminder = reminder.ReminderTime - DateTime.UtcNow;

                    embed.AddField(
                        reminder.Task,
                        $"<t:{((DateTimeOffset)reminder.ReminderTime).ToUnixTimeSeconds()}:F>\n" +
                        $"*<t:{((DateTimeOffset)reminder.ReminderTime).ToUnixTimeSeconds()}:R>*",
                        inline: false);
                }

                await FollowupAsync(embed: embed.Build(), ephemeral: true);
            }
            catch (Exception)
            {
                await FollowupAsync(
                    "❌ An error occurred while retrieving your reminders.",
                    ephemeral: true);
                throw;
            }
        }
    }
}
