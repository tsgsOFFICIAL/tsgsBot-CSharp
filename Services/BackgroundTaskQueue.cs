using System.Threading.Channels;

namespace tsgsBot_C_.Services;

/// <summary>
/// Represents a work item to be processed by the background task queue
/// </summary>
public class BackgroundTask
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Type of task for categorization
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// The work to execute
    /// </summary>
    public required Func<CancellationToken, Task> Work { get; set; }

    /// <summary>
    /// Timestamp when the task was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional description for logging purposes
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Interface for background task queue operations
/// </summary>
public interface IBackgroundTaskQueue
{
    /// <summary>
    /// Queue a task for background execution
    /// </summary>
    ValueTask QueueAsync(BackgroundTask task);

    /// <summary>
    /// Dequeue the next task to be processed
    /// </summary>
    ValueTask<BackgroundTask?> DequeueAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Implementation of background task queue using System.Threading.Channels
/// </summary>
public sealed class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<BackgroundTask> _queue;

    public BackgroundTaskQueue(int capacity)
    {
        BoundedChannelOptions options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<BackgroundTask>(options);
    }

    public async ValueTask QueueAsync(BackgroundTask task)
    {
        ArgumentNullException.ThrowIfNull(task);
        await _queue.Writer.WriteAsync(task);
    }

    public async ValueTask<BackgroundTask?> DequeueAsync(CancellationToken cancellationToken)
    {
        if (await _queue.Reader.WaitToReadAsync(cancellationToken))
        {
            _queue.Reader.TryRead(out BackgroundTask? task);
            return task;
        }

        return null;
    }
}

/// <summary>
/// Hosted service that processes background tasks from the queue
/// </summary>
public sealed class BackgroundTaskProcessor(IBackgroundTaskQueue taskQueue, ILogger<BackgroundTaskProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background task processor started");

        await foreach (BackgroundTask task in ProcessTasksAsync(stoppingToken))
        {
            try
            {
                logger.LogInformation("Processing background task {TaskId} of type '{TaskType}': {Description}",
                    task.Id, task.TaskType, task.Description ?? "No description");

                await task.Work(stoppingToken);

                TimeSpan duration = DateTime.UtcNow - task.CreatedAt;
                logger.LogInformation("Completed background task {TaskId} in {Duration}ms",
                    task.Id, duration.TotalMilliseconds);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Background task {TaskId} was cancelled", task.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing background task {TaskId} of type '{TaskType}'",
                    task.Id, task.TaskType);
            }
        }

        logger.LogInformation("Background task processor stopped");
    }

    private async IAsyncEnumerable<BackgroundTask> ProcessTasksAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            BackgroundTask? task = null;
            try
            {
                task = await taskQueue.DequeueAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown
                break;
            }

            if (task is not null)
            {
                yield return task;
            }
        }
    }
}
