namespace Backynet.Core.Abstraction;

public interface IThreadPool
{
    void Submit(object d);
}