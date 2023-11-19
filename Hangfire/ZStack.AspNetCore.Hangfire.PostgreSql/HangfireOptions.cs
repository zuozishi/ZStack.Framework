using Hangfire.PostgreSql;

namespace ZStack.AspNetCore.Hangfire.PostgreSql;

public class HangfireOptions : Hangfire.HangfireOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public PostgreSqlStorageOptions PostgreSql { get; set; } = new();
}
