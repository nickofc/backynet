using Backynet.Core.Abstraction;
using JetBrains.dotMemoryUnit;
using Xunit.Abstractions;

namespace Backynet.Tests;

public class CronTests
{
    public CronTests(ITestOutputHelper output)
    {
        DotMemoryUnitTestOutput.SetOutputMethod(output.WriteLine);
    }

    [Fact]
    [DotMemoryUnit(CollectAllocations = true)]
    public void Should_Not_Allocate_Memory()
    {
        var now = DateTimeOffset.UtcNow;

        var startingCheckpoint = dotMemory.Check();

        var cron = Cron.Parse("1 * * * *");
        cron.GetNextOccurrence(now);

        dotMemory.Check((memory =>
        {
            var trafficFrom = memory.GetTrafficFrom(startingCheckpoint);
            Assert.Equal(169, trafficFrom.AllocatedMemory.ObjectsCount);
        }));
    }
}