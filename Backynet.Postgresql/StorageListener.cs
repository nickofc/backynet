using Backynet.Core.Abstraction;
using Npgsql;

namespace Backynet.Postgresql;

internal sealed class StorageListener : IStorageListener<object>
{
    private readonly NpgsqlConnectionFactory _npgsqlConnectionFactory;

    public StorageListener(NpgsqlConnectionFactory npgsqlConnectionFactory)
    {
        _npgsqlConnectionFactory = npgsqlConnectionFactory;
    }

    public async Task Start(Func<object, Task> callback, CancellationToken cancellationToken)
    {
        await using var connection = await _npgsqlConnectionFactory.GetAsync(cancellationToken);
        connection.Notification += (_, args) =>
        {
            // huh? jak to wykonać?
            callback(args.Payload);
        };

        await using (var command = new NpgsqlCommand("LISTEN jobs.add", connection))
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        while (true)
        {
            await connection.WaitAsync(cancellationToken);
        }
    }
}