using Npgsql;

namespace Backynet.Postgresql.Tests;

public class PostgreSqlConnectionFactoryTests
{
    [Fact]
    public async Task Should_Connect_To_Database()
    {
        // todo: dodać testcontainers
        const string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres";

        var factory = new NpgsqlConnectionFactory(connectionString);
        await using var connection = await factory.GetAsync();
        await using var command = new NpgsqlCommand("SELECT 1", connection);
        await command.ExecuteReaderAsync();
    }
}