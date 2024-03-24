namespace Backynet.Core.Abstraction;

public static class CronDefaults
{
    public const char All = '*';
    public const char List = ',';
    public const char Range = '-';

    public static readonly Dictionary<int, int> MonthMap = new()
    {
        { GetHashCode("JAN"), 1 },
        { GetHashCode("FEB"), 2 },
        { GetHashCode("MAR"), 3 },
        { GetHashCode("APR"), 4 },
        { GetHashCode("MAY"), 5 },
        { GetHashCode("JUN"), 6 },
        { GetHashCode("JUL"), 7 },
        { GetHashCode("AUG"), 8 },
        { GetHashCode("SEP"), 9 },
        { GetHashCode("OCT"), 10 },
        { GetHashCode("NOV"), 11 },
        { GetHashCode("DEC"), 12 },
    };

    public static readonly Dictionary<int, int> DayOfWeekMap = new()
    {
        { GetHashCode("MON"), 1 },
        { GetHashCode("TUE"), 2 },
        { GetHashCode("WED"), 3 },
        { GetHashCode("THU"), 4 },
        { GetHashCode("FRI"), 5 },
        { GetHashCode("SAT"), 6 },
        { GetHashCode("SUN"), 7 },
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