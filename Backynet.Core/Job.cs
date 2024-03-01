namespace Backynet.Core;

public class Job
{
    public string Id { get; set; }
    public JobState JobState { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}