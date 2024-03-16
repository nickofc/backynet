using Backynet.Core.Abstraction;
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
        command.CommandText = """
                              insert into jobs (id, state, created_at, base_type, method, arguments, server_name, cron, group_name)
                              values (@id, @state, @created_at, @base_type, @method, @arguments, @server_name, @cron, @group_name);
                              """;
        command.Parameters.Add(new NpgsqlParameter("id", job.Id));
        command.Parameters.Add(new NpgsqlParameter("state", (int)job.JobState)); // todo: remove boxing
        command.Parameters.Add(new NpgsqlParameter("created_at", job.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter("base_type", job.Descriptor.BaseType));
        command.Parameters.Add(new NpgsqlParameter("method", job.Descriptor.Method));
        command.Parameters.Add(new NpgsqlParameter("arguments", _serializer.Serialize(job.Descriptor.Arguments)));
        command.Parameters.Add(new NpgsqlParameter("server_name",
            job.ServerName is null ? DBNull.Value : job.ServerName));
        command.Parameters.Add(new NpgsqlParameter("cron", job.Cron is null ? DBNull.Value : job.Cron));
        command.Parameters.Add(new NpgsqlParameter("group_name", job.GroupName is null ? DBNull.Value : job.GroupName));
        command.Parameters.Add(new NpgsqlParameter("next_occurrence_at",
            job.NextOccurrenceAt is null ? DBNull.Value : job.NextOccurrenceAt));
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<Job?> Get(Guid jobId, CancellationToken cancellationToken = default)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = """
                              select id, state, created_at, base_type, method, arguments, server_name, cron, group_name, next_occurrence_at
                              from jobs where id = @id
                              """;
        command.Parameters.Add(new NpgsqlParameter("id", jobId));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var read = await reader.ReadAsync(cancellationToken);
        Job? job = null;

        if (read)
        {
            job = ParseJobRow(reader);
        }

        return job;
    }

    public async Task Update(Guid jobId, Job job, CancellationToken cancellationToken = default)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = """
                              update jobs
                              set
                                  state = @state,
                                  created_at = @created_at,
                                  base_type = @base_type,
                                  method = @method,
                                  arguments = @arguments,
                                  server_name = @server_name,
                                  cron = @cron,
                                  group_name = @group_name,
                                  next_occurrence_at = @next_occurrence_at
                              where jobs.id = @id;
                              """;
        command.Parameters.Add(new NpgsqlParameter("id", job.Id));
        command.Parameters.Add(new NpgsqlParameter("state", (int)job.JobState)); // todo: remove boxing
        command.Parameters.Add(new NpgsqlParameter("created_at", job.CreatedAt));
        command.Parameters.Add(new NpgsqlParameter("base_type", job.Descriptor.BaseType));
        command.Parameters.Add(new NpgsqlParameter("method", job.Descriptor.Method));
        command.Parameters.Add(new NpgsqlParameter("arguments", _serializer.Serialize(job.Descriptor.Arguments)));
        command.Parameters.Add(new NpgsqlParameter("server_name", job.ServerName));
        command.Parameters.Add(new NpgsqlParameter("cron", job.Cron));
        command.Parameters.Add(new NpgsqlParameter("group_name", job.GroupName));
        command.Parameters.Add(new NpgsqlParameter("next_occurrence_at", job.NextOccurrenceAt));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Job>> Acquire(string serverName, int count,
        CancellationToken cancellationToken = default)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        await using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = """
                              update jobs
                              set
                                  server_name = @server_name,
                                  state = @state
                              where jobs.id in (select id
                                                from jobs
                                                where jobs.server_name is null and state = 2
                                                order by id
                                                limit @limit)
                              RETURNING id, state, created_at, base_type, method, arguments, server_name, cron, group_name, next_occurrence_at
                              """;
        command.Parameters.Add(new NpgsqlParameter("server_name", serverName));
        command.Parameters.Add(new NpgsqlParameter("state", (int)JobState.Scheduled));
        command.Parameters.Add(new NpgsqlParameter<int>("limit", count));

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var jobs = new List<Job>((int)reader.Rows);

        while (await reader.ReadAsync(cancellationToken))
        {
            var job = ParseJobRow(reader);
            jobs.Add(job);
        }

        return jobs;
    }

    private Job ParseJobRow(NpgsqlDataReader reader)
    {
        var id = reader.GetGuid(0);
        var state = reader.GetInt32(1);
        var created = reader.GetDateTime(2);

        var baseType = reader.GetString(3);
        var method = reader.GetString(4);
        var arguments = _serializer.Deserialize<object[]>(reader.GetString(5));

        string? serverName = null;

        if (!reader.IsDBNull(6))
        {
            serverName = reader.GetString(6);
        }

        string? cron = null;

        if (!reader.IsDBNull(7))
        {
            cron = reader.GetString(7);
        }

        string? groupName = null;

        if (!reader.IsDBNull(8))
        {
            groupName = reader.GetString(8);
        }

        DateTimeOffset? nextOccurrenceAt = null;

        if (!reader.IsDBNull(9))
        {
            nextOccurrenceAt = reader.GetDateTime(9);
        }

        return new Job
        {
            Id = id,
            JobState = (JobState)state,
            CreatedAt = created,
            Descriptor = new JobDescriptor(baseType, method, arguments),
            Cron = cron,
            ServerName = serverName,
            GroupName = groupName,
            NextOccurrenceAt = nextOccurrenceAt
        };
    }
}