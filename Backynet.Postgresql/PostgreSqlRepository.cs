using Backynet.Core;
using Backynet.Core.Abstraction;
using Npgsql;

namespace Backynet.Postgresql;

internal class PostgreSqlRepository : IStorage, IStorageListener
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
        command.CommandText = "insert into jobs (id, state, created_at, base_type, method, arguments) " +
                              "values (@id, @state, @created_at, @base_type, @method, @arguments);";
        command.Parameters.Add(new NpgsqlParameter("id", job.Id));
        command.Parameters.Add(new NpgsqlParameter("state", (int)job.JobState));
        command.Parameters.Add(new NpgsqlParameter("created_at", job.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter("base_type", job.Descriptor.BaseType));
        command.Parameters.Add(new NpgsqlParameter("method", job.Descriptor.Method));
        command.Parameters.Add(new NpgsqlParameter("arguments", _serializer.Serialize(job.Descriptor.Arguments)));
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Job?> Get(Guid jobId, CancellationToken cancellationToken = default)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = "select id, state, created_at, base_type, method, arguments " +
                              "from jobs where id = @id";
        command.Parameters.Add(new NpgsqlParameter("id", jobId));
        
        var reader = await command.ExecuteReaderAsync(cancellationToken);
        var read = await reader.ReadAsync(cancellationToken);
        Job? job = null;
        
        if (read)
        {
            var id = reader.GetGuid(0);
            var state = reader.GetInt32(1);
            var created = reader.GetDateTime(2);
            var baseType = reader.GetString(3);
            var method = reader.GetString(4);
            var args = reader.GetString(5);

            job = new Job
            {
                Id = id,
                JobState = (JobState) state,
                CreatedAt = created,
                Descriptor = new JobDescriptor(baseType, method, _serializer.Deserialize<object[]>(args))
            };
        }
        
        return job;
    }

    public event EventHandler<string> OnItemAdded;

    public async Task Start(CancellationToken cancellationToken)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        connection.Notification += (_, a) => { OnItemAdded.Invoke(this, a.Channel); };

        await using (var command = new NpgsqlCommand("LISTEN channel", connection))
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        while (true)
        {
            await connection.WaitAsync(cancellationToken);
        }
    }
}