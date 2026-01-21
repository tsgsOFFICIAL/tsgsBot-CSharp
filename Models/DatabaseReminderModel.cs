namespace tsgsBot_C_.Models;

/// <summary>
/// Represents a persistent reminder stored in the database.
/// </summary>
public sealed class DatabaseReminderModel
{
    /// <summary>
    /// Unique identifier for the reminder.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user who created the reminder (Discord user ID).
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// The task or reminder text.
    /// </summary>
    public required string Task { get; set; }

    /// <summary>
    /// The time when the reminder should be sent (UTC).
    /// </summary>
    public DateTime ReminderTime { get; set; }

    /// <summary>
    /// Whether the reminder has been sent.
    /// </summary>
    public bool HasSent { get; set; }

    /// <summary>
    /// Timestamp when the reminder was created (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
