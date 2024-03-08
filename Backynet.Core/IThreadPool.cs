namespace Backynet.Core;

public interface IThreadPool
{
    void Submit(object d);
}