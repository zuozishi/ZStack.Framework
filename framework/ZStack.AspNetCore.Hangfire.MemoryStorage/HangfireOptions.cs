using Hangfire.MemoryStorage;

namespace ZStack.AspNetCore.Hangfire.MemoryStorage;

public class HangfireOptions : Hangfire.HangfireOptions
{
    public MemoryStorageOptions MemoryStorage { get; set; } = new();
}
