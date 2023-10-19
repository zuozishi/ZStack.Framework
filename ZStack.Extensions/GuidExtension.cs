namespace ZStack.Extensions;

public static class GuidExtension
{
    public static string ToUUID32(this Guid guid)
        => guid.ToString().Replace("-", string.Empty);
}
