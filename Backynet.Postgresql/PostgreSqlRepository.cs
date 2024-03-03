using Backynet.Core;
using Backynet.Core.Abstraction;
using Newtonsoft.Json;
using Npgsql;

namespace Backynet.Postgresql;

internal class PostgreSqlRepository : IStorage
{
    private readonly NpgsqlConnectionFactory _npgsqlConnectionFactory;
    private readonly ISerializer _serializer;

    public PostgreSqlRepository(NpgsqlConnectionFactory npgsqlConnectionFactory, ISerializer serializer)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
        _serializer = serializer;
    }

    public async Task Add(Job job, CancellationToken cancellationToken = default)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = "insert into jobs (id, state, created_at, invokable) values (@id, @state, @created_at, @invokable);";
        command.Parameters.Add(new NpgsqlParameter("id", job.Id));
        command.Parameters.Add(new NpgsqlParameter("state", (int)job.JobState));
        command.Parameters.Add(new NpgsqlParameter("created_at", job.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter("invokable", _serializer.Serialize(job.Invokable)));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}