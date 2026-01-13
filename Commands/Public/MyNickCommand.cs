using Discord.Interactions;
using Discord.WebSocket;
using Discord;

namespace tsgsBot_C_.Commands.Public
{
    public sealed class MyNickCommand : LoggedCommandModule
    {
        [SlashCommand("mynick", "Change your nickname in this server")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.ChangeNickname)]
        public async Task NickAsync(
            [Summary("nickname", "The new nickname (leave blank to reset)")] string? nickname = null)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("nickname", nickname ?? "(reset)"));

            SocketGuildUser user = Context.Guild.GetUser(Context.User.Id);

            try
            {
                await user.ModifyAsync(props => props.Nickname = nickname);

                if (nickname == null)
                    await FollowupAsync($"Reset nickname.", ephemeral: true);
                else
                    await FollowupAsync($"Changed nickname to **{nickname}**.", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error changing nickname for user {user.Id}: {error}");
                await FollowupAsync("Failed to change your nickname. Ensure I have the proper permissions and role hierarchy.", ephemeral: true);
            }
        }
    }
}