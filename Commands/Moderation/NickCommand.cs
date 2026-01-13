using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Commands.Moderation
{
    public sealed class NickCommand : LoggedCommandModule
    {
        [SlashCommand("nick", "Change a user's nickname in this server")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.ManageNicknames)]
        public async Task NickAsync(
            [Summary("user", "The user whose nickname to change")] IGuildUser user,
            [Summary("nickname", "The new nickname (leave blank to reset)")] string? nickname = null)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("user", user), ("nickname", nickname ?? "(reset)"));
            try
            {
                await user.ModifyAsync(props => props.Nickname = nickname);

                if (nickname == null)
                    await FollowupAsync($"Reset nickname for {user.Mention}.", ephemeral: true);
                else
                    await FollowupAsync($"Changed nickname for {user.Mention} to **{nickname}**.", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error changing nickname for user {user.Id}: {error}");
                await FollowupAsync("Failed to change the user's nickname. Ensure I have the proper permissions and role hierarchy.", ephemeral: true);
            }
        }
    }
}