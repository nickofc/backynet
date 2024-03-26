namespace Backynet.Core.Abstraction;

public interface IJobRunner
{
    Task Run(Job job);
}