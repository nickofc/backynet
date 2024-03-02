using Backynet.Core.Abstraction;
using Npgsql;

namespace Backynet.Postgresql;

internal class PostgreSqlRepository
{
    private readonly NpgsqlConnectionFactory _npgsqlConnectionFactory;

    public PostgreSqlRepository(NpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task Add(Job job)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync();
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = "insert into jobs (id, state, created_at) values (@id, @state, @created_at);";
        command.Parameters.Add(new NpgsqlParameter("id", job.Id));
        command.Parameters.Add(new NpgsqlParameter("state", (int) job.JobState));
        command.Parameters.Add(new NpgsqlParameter("created_at", job.CreatedAt));
        await command.ExecuteNonQueryAsync();
    }
}