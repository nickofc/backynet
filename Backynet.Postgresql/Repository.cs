using Backynet.Core;
using Backynet.Core.Abstraction;

namespace Backynet.Postgresql;

public class Repository
{
    private readonly NpgsqlConnectionFactory _npgsqlConnectionFactory;


    public async Task Add(Job job)
    {
        await using var npgsqlConnection = await _npgsqlConnectionFactory.GetAsync();
    }

    public async Task Update(string jobId, Job job)
    {
    }
}