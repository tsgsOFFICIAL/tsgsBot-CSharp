namespace tsgsBot_C_.Models
{
    /// <summary>
    /// Represents a poll stored in the database, including its metadata, question, possible answers, and status
    /// information.
    /// </summary>
    /// <param name="Id">The unique identifier for the poll.</param>
    /// <param name="MessageId">The identifier of the message associated with the poll.</param>
    /// <param name="ChannelId">The identifier of the channel where the poll is posted.</param>
    /// <param name="GuildId">The identifier of the guild (server) in which the poll exists.</param>
    /// <param name="Question">The question presented to users in the poll.</param>
    /// <param name="Answers">A list of possible answer options for the poll.</param>
    /// <param name="Emojis">A list of emojis corresponding to each answer option. Each emoji is used for voting.</param>
    /// <param name="EndTime">The date and time when the poll is scheduled to end, in UTC.</param>
    /// <param name="HasEnded">true if the poll has ended; otherwise, false.</param>
    /// <param name="CreatedAt">The date and time when the poll was created, in UTC.</param>
    /// <param name="CreatedByUserId">The identifier of the user who created the poll.</param>
    public record DatabasePollModel(
        int Id,
        string MessageId,
        string ChannelId,
        string GuildId,
        string Question,
        List<string> Answers,
        List<string> Emojis,
        DateTime EndTime,
        bool HasEnded,
        DateTime CreatedAt,
        ulong CreatedByUserId
    );
}