namespace Backynet.Postgresql;

public class BackynetPosgresqlServerOptions
{
    public bool IsAutomaticMigrationEnabled { get; set; }
    public string ConnectionString { get; set; }
}