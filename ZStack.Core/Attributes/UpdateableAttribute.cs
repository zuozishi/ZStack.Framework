namespace ZStack.Core.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class UpdateableAttribute : Attribute
{
    public bool IsJson { get; set; }

    public bool IsIgnore { get; set; }
}
