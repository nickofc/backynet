namespace Backynet.Core.Abstraction;

public static class CronUtil
{
    public static TimeSpan GetNextOccurrence(string cron)
    {
        return TimeSpan.FromDays(1);
    }
}

public class Cron
{
    public static TimeSpan Parse(ReadOnlySpan<char> value)
    {
        Span<char> dayOfWeek = stackalloc char[3];
        Span<char> month = stackalloc char[3];
        Span<char> dayOfMonth = stackalloc char[2];
        Span<char> hour = stackalloc char[2];
        Span<char> minute = stackalloc char[2];

        var length = value.Length;
        var index = length - 1;
        
        
        return TimeSpan.Zero;
    }
}