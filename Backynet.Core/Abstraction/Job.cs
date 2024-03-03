namespace Backynet.Core.Abstraction;

public class Job
{
    public Guid Id { get; set; }
    public JobState JobState { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Invokable Invokable { get; set; }

    public static Job Create()
    {
        return new Job
        {
            Id = Guid.NewGuid(),
            JobState = JobState.Created,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}