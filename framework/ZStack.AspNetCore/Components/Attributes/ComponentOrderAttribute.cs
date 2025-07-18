namespace System;

[AttributeUsage(AttributeTargets.Class)]
public class ComponentOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}
