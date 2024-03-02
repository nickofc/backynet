namespace Backynet.Postgresql;

public class BackynetPosgreSqlServerOptions
{
    public bool IsAutomaticMigrationEnabled { get; set; }
    public string ConnectionString { get; set; }
}