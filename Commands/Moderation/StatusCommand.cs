using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Commands.Moderation
{
    public sealed class StatusCommand : LoggedCommandModule
    {
        [SlashCommand("status", "Temporarily change the bot's status message and type")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task StatusAsync(
            [Summary("type", "The type of status")]
            [Choice("Playing", "Playing")]
            [Choice("Listening to", "Listening")]
            [Choice("Watching", "Watching")]
            [Choice("Competing in", "Competing")]
            [Choice("Streaming", "Streaming")]
            string type,
            [Summary("message", "The status message to set")] string message)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("message", message), ("type", type));

            // Map type string to Discord ActivityType enum
            ActivityType activityType = type switch
            {
                "Playing" => ActivityType.Playing,
                "Listening" => ActivityType.Listening,
                "Watching" => ActivityType.Watching,
                "Competing" => ActivityType.Competing,
                "Streaming" => ActivityType.Streaming,
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };

            try
            {
                IActivity activity;
                if (activityType == ActivityType.Streaming)
                {
                    // Streaming requires a URL; using a placeholder or null might not work, but to match JS logic
                    activity = new StreamingGame(message, null); // Note: This may not display properly without a valid URL
                }
                else
                {
                    activity = new Game(message, activityType);
                }

                await Context.Client.SetActivityAsync(activity);
                await FollowupAsync($"Status set to _{type}_ **{message}**", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error setting status: {error}");
                await FollowupAsync("Failed to update status.", ephemeral: true);
            }
        }
    }
}