﻿namespace Backynet.Abstraction;

public interface ISystemClock
{
    DateTimeOffset UtcNow
    {
        get { return DateTimeOffset.UtcNow; }
    }
}