using Backynet.Core;
using Backynet.Core.Abstraction;
using Backynet.Tests;

namespace Backynet.Postgresql.Tests;

public class PostgresRepositoryTests
{
    [Fact]
    public async Task Should_Insert_Job_To_Database_When_Add_Is_Called()
    {
        // arrange

        var expectedJob = Job.Empty();
        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

        //act 

        await repository.Add(expectedJob);

        // assert

        var actualJob = await repository.Get(expectedJob.Id);

        Assert.Equal(expectedJob.Id, actualJob.Id);
        Assert.Equal(expectedJob.JobState, actualJob.JobState);
        Assert.Equal(expectedJob.Descriptor.BaseType, actualJob.Descriptor.BaseType);
        Assert.Equal(expectedJob.Descriptor.Method, actualJob.Descriptor.Method);
        Assert.Equal(expectedJob.Descriptor.Arguments, actualJob.Descriptor.Arguments);
    }

    [Fact]
    public async Task Should_Acquire_Job_When_Acquire_Is_Called()
    {
        // arrange

        var serverName = Guid.NewGuid().ToString();
        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

        for (var i = 0; i < 10; i++)
        {
            var emptyJob = Job.Empty();
            await repository.Add(emptyJob);
        }

        //act 

        var jobs = await repository.Acquire(serverName, 2);

        // assert

        Assert.Equal(2, jobs.Count);
        Assert.True(jobs.All(x => x.ServerName == serverName));
    }
}