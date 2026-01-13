using Discord.Interactions;
using Discord.WebSocket;
using System.Text;
using Discord;

namespace tsgsBot_C_
{
    public abstract class LoggedCommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        /// <summary>
        /// Logs this command execution to the fixed log channel.
        /// Pass parameters as (name, value) pairs.
        /// </summary>
        protected async Task LogCommandAsync(params (string Name, object? Value)[] options)
        {
            SocketTextChannel? logChannel = Context.Guild.TextChannels.FirstOrDefault(channel => channel.Name == "command-log");

            if (logChannel == null)
            {
                await RespondAsync("⚠️ Log channel not found — command logged internally only.", ephemeral: true);
                return;
            }
            
            StringBuilder sb = new StringBuilder();

            foreach ((string? name, object? value) in options)
            {
                sb.Append($" *{name}:* ");

                sb.Append(value switch
                {
                    null => "**null**",
                    IGuildUser guildUser => guildUser.Mention, // IGuildUser inherits IUser, so this must come before IUser
                    IUser user => user.Mention,
                    IRole role => role.Mention,
                    IChannel channel => channel.Id,
                    ulong id => $"<@!{id}>",
                    _ => $"**{value}**"
                });
            }

            string commandName = Context.Interaction.GetType().GetProperty("CommandName")?.GetValue(Context.Interaction) as string ?? "unknown";
            string logMessage = $"<@{Context.User.Id}> {(Context.Interaction.Type == InteractionType.ModalSubmit ? "**Submitted a modal**" : $"used **/{commandName}")}**{sb} in <#{Context.Channel.Id}>";

            try
            {
                await logChannel.SendMessageAsync(logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log command to channel {logChannel}: {ex.Message}");
                Console.WriteLine(logMessage);
            }
        }

        protected Task LogCommandAsync() => LogCommandAsync([]);
    }
}