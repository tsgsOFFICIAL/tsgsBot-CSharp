using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Bot.Commands.Moderation
{
    public sealed class DMCommand : LoggedCommandModule
    {
        [SlashCommand("dm", "Send a direct message to a user")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task DMAsync(
            [Summary("user", "The user to send the DM to")] IUser user,
            [Summary("message", "The message content to send")] string message)
        {
            await DeferAsync(ephemeral: true);

            await LogCommandAsync(("user", user), ("message", message));
            
            try
            {
                IDMChannel dmChannel = await user.CreateDMChannelAsync();
                await dmChannel.SendMessageAsync(message);
                await FollowupAsync($"Successfully sent a DM to {user.Mention}.", ephemeral: true);
            }
            catch (Exception ex)
            {
                await FollowupAsync($"Failed to send DM to {user.Mention}: {ex.Message}", ephemeral: true);
            }
        }
    }
}