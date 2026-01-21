using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Bot.Commands.Moderation
{
    public sealed class SayCommand : LoggedCommandModule
    {
        [SlashCommand("say", "Make the bot say something, somewhere")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.ManageMessages)]
        public async Task SayAsync(
            [Summary("message", "The message for the bot to say")] string message,
            [Summary("channel", "The channel to send the message in")] IMessageChannel? channel = null)
        {
            await DeferAsync(ephemeral: true);

            if (channel == null)
                await LogCommandAsync(("message", message));
            else
                await LogCommandAsync(("channel", channel), ("message", message));

            channel ??= Context.Channel;

            try
            {
                await channel.SendMessageAsync(message);
                await FollowupAsync($"Message sent in <#{channel.Id}>.", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error sending message in channel {channel.Id}: {error}");
                await FollowupAsync("Failed to send message in the specified channel.", ephemeral: true);
            }
        }
    }
}