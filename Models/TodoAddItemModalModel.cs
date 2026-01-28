using Discord.Interactions;
using Discord;

namespace tsgsBot_C_.Models
{
    public sealed class TodoAddItemModalModel : IModal
    {
        public string Title => "Add Todo Item";

        [InputLabel("Item")]
        [ModalTextInput("todo_item", TextInputStyle.Short, maxLength: 120)]
        public required string Item { get; set; }
    }
}
