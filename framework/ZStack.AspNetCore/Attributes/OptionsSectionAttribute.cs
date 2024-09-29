namespace ZStack.AspNetCore.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class OptionsSectionAttribute(string key) : Attribute
{
    public string Key => key;
}
