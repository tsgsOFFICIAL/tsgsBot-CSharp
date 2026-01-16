using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Models
{
    public sealed class GiveawayModalModel : IModal
    {
        public string Title => "Create Your Giveaway";

        [InputLabel("What's the prize")]
        [ModalTextInput("prize", TextInputStyle.Short)]
        public required string Prize { get; set; }

        [InputLabel("How many can win")]
        [ModalTextInput("winners", TextInputStyle.Short)]
        public required string Winners { get; set; }

        [InputLabel("Reaction ReactionEmoji")]
        [ModalTextInput("reaction_emoji", TextInputStyle.Short)]
        public required string ReactionEmoji { get; set; }
    }
}