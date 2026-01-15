using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Commands.Public
{
    public sealed class InviteCommand() : LoggedCommandModule
    {
        [SlashCommand("invite", "Get the invite URL")]
        [DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
        public async Task InviteAsync()
        {
            await LogCommandAsync();

            await RespondAsync("Here is the invite link: https://discord.gg/Cddu5aJ", ephemeral: true);
        }
    }
}