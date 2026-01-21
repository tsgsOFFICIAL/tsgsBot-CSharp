using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Bot.Commands.Moderation
{
    public sealed class RemoveRoleCommand : LoggedCommandModule
    {
        [SlashCommand("role-remove", "Removes a role from a user")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.ManageRoles)]
        public async Task AddRoleAsync(
            [Summary("user", "The user to remove the role from")] IGuildUser user,
            [Summary("role", "The role to remove")] IRole role)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("user", user), ("role", role));
            try
            {
                await user.RemoveRoleAsync(role);
                await FollowupAsync($"Removed role **{role.Name}** to {user.Mention}.", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error removing role {role.Id} from user {user.Id}: {error}");
                await FollowupAsync("Failed to remove the role from the user. Ensure I have the proper permissions and role hierarchy.", ephemeral: true);
            }
        }
    }
}