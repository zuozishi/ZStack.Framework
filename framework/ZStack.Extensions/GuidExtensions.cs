namespace System;

public static class GuidExtensions
{
    public static string ToUUID32(this Guid guid)
        => guid.ToString("N");
}
