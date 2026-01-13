using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Commands.Moderation
{
    public sealed class AddRoleCommand : LoggedCommandModule
    {
        [SlashCommand("role-add", "Add a role to a user")]
        [CommandContextType(InteractionContextType.Guild)]
        [DefaultMemberPermissions(GuildPermission.ManageRoles)]
        public async Task AddRoleAsync(
            [Summary("user", "The user to add the role to")] IGuildUser user,
            [Summary("role", "The role to add")] IRole role)
        {
            await DeferAsync(ephemeral: true);
            await LogCommandAsync(("user", user), ("role", role));
            try
            {
                await user.AddRoleAsync(role);
                await FollowupAsync($"Added role **{role.Name}** to {user.Mention}.", ephemeral: true);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error adding role {role.Id} to user {user.Id}: {error}");
                await FollowupAsync("Failed to add the role to the user. Ensure I have the proper permissions and role hierarchy.", ephemeral: true);
            }
        }
    }
}