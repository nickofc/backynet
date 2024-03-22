namespace Backynet.Core.Abstraction;

public ref struct Cron
{
    private readonly int _second;
    private readonly int _minute;
    private readonly int _hour;
    private readonly int _dayOfMonth;
    private readonly int _month;
    private readonly int _dayOfWeek;

    public Cron(int second, int minute, int hour, int dayOfMonth, int month, int dayOfWeek)
    {
        _second = second;
        _minute = minute;
        _hour = hour;
        _dayOfMonth = dayOfMonth;
        _month = month;
        _dayOfWeek = dayOfWeek;
    }

    public static Cron Parse(ReadOnlySpan<char> expression, CronType cronType = CronType.Standard)
    {
        if (cronType == CronType.Extended)
        {
            throw new NotSupportedException("Extended cron is not yet supported.");
        }

        var second = 0;
        var minute = 0;
        var hour = 0;
        var dayOfMonth = 0;
        var month = 0;
        var dayOfWeek = 0;

        var position = 0;

        while (char.IsWhiteSpace(expression[position]))
        {
            position++;
        }

        return new Cron();
    }

    public DateTimeOffset GetNextOccurrence(DateTimeOffset now)
    {
        return now;
    }
}