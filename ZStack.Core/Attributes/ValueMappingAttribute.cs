namespace ZStack.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ValueMappingAttribute(object? origin, object? value) : Attribute
{
    public object? Origin { get; set; } = origin;

    public object? Value { get; set; } = value;
}
