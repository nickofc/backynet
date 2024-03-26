namespace Backynet.Core.Abstraction;

public readonly ref struct Cron
{
    private readonly MinutesField _minutesField;
    private readonly HoursField _hoursField;

    private Cron(MinutesField minutesField, HoursField hoursField)
    {
        _minutesField = minutesField;
        _hoursField = hoursField;
    }

    public static Cron Parse(ReadOnlySpan<char> input, CronType cronType = CronType.Standard)
    {
        if (cronType == CronType.Extended)
        {
            throw new NotSupportedException("Extended cron is not yet supported.");
        }

        // "0 22 * * 1-5"    = At 22:00 on every day-of-week from Monday through Friday.
        // "23 0-20/2 * * *" = At minute 23 past every 2nd hour from 0 through 20.
        // "5 4 * * sun"     = At 04:05 on Sunday.
        // "0 0 1,15 * 3"    = At 00:00 on day-of-month 1 and 15 and on Wednesday.

        var reader = new CronReader(input);

        var minutesField = new MinutesField(reader.GetNextField());
        var hoursField = new HoursField(reader.GetNextField());

        return new Cron(minutesField, hoursField);
    }

    public DateTimeOffset GetNextOccurrence(DateTimeOffset now)
    {
        now = now.AddSeconds(1);

        while (true)
        {
            if (!_minutesField.IsValid(now))
            {
                now = _minutesField.GetNext(now);
                continue;
            }

            if (!_hoursField.IsValid(now))
            {
                now = _hoursField.GetNext(now);
                continue;
            }

            break;
        }

        return now;
    }

    private ref struct CronReader
    {
        private readonly ReadOnlySpan<char> _input;

        public CronReader(ReadOnlySpan<char> input)
        {
            _input = input;
        }

        private int _position = 0;

        public Field GetNextField()
        {
            while (_position < _input.Length && _input[_position] == ' ')
            {
                _position++;
            }

            var start = _position;

            while (_position < _input.Length && _input[_position] != ' ')
            {
                _position++;
            }

            return new Field(_input.Slice(start, _position - start));
        }
    }

    private ref struct Field
    {
        public ReadOnlySpan<char> Value { get; }
        public bool IsEmpty { get; }

        public Field(ReadOnlySpan<char> value)
        {
            Value = value;
            IsEmpty = value.Length == 0;
        }

        public void Read()
        {
            Span<Range> range = stackalloc Range[Value.Length];
            Value.Split(range, ',');
            
            // */5,
            // 1-5,
            // 5 
        }
    }

    private readonly ref struct MinutesField
    {
        private readonly Field _field;

        public MinutesField(Field field)
        {
            _field = field;
        }

        public bool IsValid(DateTimeOffset date)
        {
            Span<Range> range = stackalloc Range[_field.Value.Length];
            
            if (_field.Value.Split(range, ',') > 0)
            {
                
            }
            
            
            
            // 	* , - / 
            return false;
        }

        public DateTimeOffset GetNext(DateTimeOffset date)
        {
            return date;
        }
    }

    private readonly ref struct HoursField
    {
        private readonly Field _field;

        public HoursField(Field field)
        {
            _field = field;
        }

        public bool IsValid(DateTimeOffset now)
        {
            throw new NotImplementedException();
        }

        public DateTimeOffset GetNext(DateTimeOffset now)
        {
            throw new NotImplementedException();
        }
    }
}