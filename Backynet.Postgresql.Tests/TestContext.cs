namespace Backynet.Postgresql.Tests;

public static class TestContext
{
    // todo: dodać testcontainers
    public const string ConnectionString = "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres";
}