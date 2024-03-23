namespace Backynet.Core.Abstraction;

public static class CronDefaults
{
    public const char All = '*';
    public const char List = ',';
    public const char Range = '-';

    public static readonly Dictionary<int, int> MonthMap = new()
    {
        { GetHashCode("JAN"), 0 },
        { GetHashCode("FEB"), 1 },
        { GetHashCode("MAR"), 2 },
        { GetHashCode("APR"), 3 },
        { GetHashCode("MAY"), 4 },
        { GetHashCode("JUN"), 5 },
        { GetHashCode("JUL"), 6 },
        { GetHashCode("AUG"), 7 },
        { GetHashCode("SEP"), 8 },
        { GetHashCode("OCT"), 9 },
        { GetHashCode("NOV"), 10 },
        { GetHashCode("DEC"), 11 },
    };

    public static readonly Dictionary<int, int> DayOfWeekMap = new()
    {
        { GetHashCode("MON"), 0 },
        { GetHashCode("TUE"), 1 },
        { GetHashCode("WED"), 2 },
        { GetHashCode("THU"), 3 },
        { GetHashCode("FRI"), 4 },
        { GetHashCode("SAT"), 5 },
        { GetHashCode("SUN"), 6 },
    };
    
    private static int GetMonth(ref ReadOnlySpan<char> input)
    {
        if (input.Length > 3)
        {
            return -1;
        }

        var hashCode = GetHashCode(input);
        return MonthMap.GetValueOrDefault(hashCode, -1);
    }
    
    private static int GetHashCode(ReadOnlySpan<char> input)
    {
        var hashCode = new HashCode();

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < input.Length; i++)
        {
            var @char = input[i];
            hashCode.Add(char.ToUpper(@char));
        }

        return hashCode.ToHashCode();
    }
}