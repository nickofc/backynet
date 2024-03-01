namespace Backynet.Core.Abstraction;

public class Job
{
    public Guid Id { get; set; }
    public JobState JobState { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}