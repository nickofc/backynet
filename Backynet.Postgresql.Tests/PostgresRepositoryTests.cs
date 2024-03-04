using Backynet.Core;
using Backynet.Core.Abstraction;

namespace Backynet.Postgresql.Tests;

public class PostgresRepositoryTests
{
    [Fact]
    public async Task Should_Add_Job_To_Database_When_Add_Invoked()
    {
        const string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres";

        var job = Job.Create();

        var factory = new NpgsqlConnectionFactory(connectionString);
        var serializer = new DefaultJsonSerializer();
        var repository = new PostgreSqlRepository(factory, serializer);

        await repository.Add(job);
    }
}