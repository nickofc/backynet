namespace Backynet.Tests;

public static class TestContext
{
    // todo: dodać testcontainers
    public static string ConnectionString { get; } =
        Environment.GetEnvironmentVariable("BACKYNET_DB") ??
        throw new InvalidOperationException("Environment variable not found.");
}