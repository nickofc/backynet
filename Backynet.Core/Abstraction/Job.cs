using System.Linq.Expressions;

namespace Backynet.Core.Abstraction;

public class Job
{
    public Guid Id { get; set; }
    public JobState JobState { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IJobDescriptor Descriptor { get; set; }
    public string? ServerName { get; set; }
    public string? Cron { get; set; }
    public string? GroupName { get; set; }
    public DateTimeOffset? NextOccurrenceAt { get; set; }

    public static Job Create(Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var methodMetadata = JobDescriptor.Create(expression);
        return Create(methodMetadata);
    }

    public static Job Create(IJobDescriptor jobDescriptor)
    {
        ArgumentNullException.ThrowIfNull(jobDescriptor);

        var job = new Job
        {
            Id = Guid.NewGuid(),
            JobState = JobState.Created,
            CreatedAt = DateTimeOffset.UtcNow,
            Descriptor = jobDescriptor
        };

        return job;
    }

    public static Job Empty()
    {
        return Create(JobDescriptor.Empty());
    }
}