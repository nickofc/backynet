namespace Backynet.Tests;

public static class TestContext
{
    // todo: dodać testcontainers
    public static string ConnectionString { get; } = Environment.GetEnvironmentVariable("BACKYNET_CONNECTION_STRING") ??
                                                     throw new InvalidOperationException("Unable to load connection string.");
}