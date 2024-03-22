namespace Backynet.Core.Abstraction;

public static class CronUtil
{
    public static TimeSpan GetNextOccurrence(string cron)
    {
        return TimeSpan.FromDays(1);
    }
}