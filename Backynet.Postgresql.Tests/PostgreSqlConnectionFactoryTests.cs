using Npgsql;

namespace Backynet.Postgresql.Tests;

public class PostgreSqlConnectionFactoryTests
{
    [Fact]
    public async Task Should_Return_Opened_Connection_When_GetAsync_Is_Called()
    {
        var factory = new NpgsqlConnectionFactory(TestContext.ConnectionString);
        await using var connection = await factory.GetAsync();
        await using var command = new NpgsqlCommand("SELECT 1", connection);
        await command.ExecuteReaderAsync();
    }
}