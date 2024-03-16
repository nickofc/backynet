namespace Backynet.Core.Abstraction;

public class Group
{
    public Guid Id { get; set; }
    public string GroupName { get; set; }
    public int MaxConcurrentThreads { get; set; }
}