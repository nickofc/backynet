using Npgsql;

namespace Backynet.Postgresql;

internal sealed class Migration
{
    private readonly NpgsqlConnectionFactory _connectionFactory;

    public Migration(NpgsqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Perform(CancellationToken cancellationToken)
    {
        var scripts = new List<string>();

        await using var connection = await _connectionFactory.GetAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        foreach (var script in scripts)
        {
            await using var command = new NpgsqlCommand(script, connection, transaction);
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
    }
}