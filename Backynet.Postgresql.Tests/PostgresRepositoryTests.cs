using Backynet.Core;
using Backynet.Core.Abstraction;

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
}