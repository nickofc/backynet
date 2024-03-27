using Npgsql;

namespace Backynet.PostgreSql;

public static class DatabaseExtensions
{
    public static async Task DeleteAllJobs(NpgsqlConnection connection)
    {
        await using var command = new NpgsqlCommand("delete from jobs where 1=1", connection);
        await command.ExecuteNonQueryAsync();
    }
}