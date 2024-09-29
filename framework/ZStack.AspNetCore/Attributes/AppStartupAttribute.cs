namespace ZStack.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AppStartupAttribute : Attribute
{
    public int Order { get; set; } = 0;
}
