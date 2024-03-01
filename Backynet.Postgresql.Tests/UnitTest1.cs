using Npgsql;

namespace Backynet.Postgresql.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        // todo: dodać testcontainers
        const string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres";

        var factory = new NpgsqlConnectionFactory(connectionString);
        await using var connection = await factory.GetAsync();
        await using var command = new NpgsqlCommand("SELECT 1", connection);
        await command.ExecuteReaderAsync();
    }
}