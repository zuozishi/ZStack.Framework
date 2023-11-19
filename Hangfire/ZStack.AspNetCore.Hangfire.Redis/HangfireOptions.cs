using Hangfire.Redis.StackExchange;

namespace ZStack.AspNetCore.Hangfire.Redis;

public class HangfireOptions : Hangfire.HangfireOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";

    public RedisStorageOptions Redis { get; set; } = new();
}
