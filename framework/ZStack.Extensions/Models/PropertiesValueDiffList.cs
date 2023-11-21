using System.Reflection;
using ZStack.Core.Attributes;

namespace ZStack.Extensions.Models;

/// <summary>
/// 属性值差异列表
/// </summary>
public class PropertiesValueDiffList(Type type)
{
    public Type Type { get; } = type;

    private List<ValueDiffItem> InternalItems { get; set; } = [];

    public IReadOnlyList<ValueDiffItem> Items => InternalItems;

    public bool HasChange => InternalItems.Count > 0;

    public void Add(PropertyInfo propertyInfo, object? value, object? oldValue)
    {
        var valueMappingAttrs = propertyInfo.GetCustomAttributes<ValueMappingAttribute>();
        foreach (var attr in valueMappingAttrs)
        {
            if (attr.Origin?.ToString() == value?.ToString())
                value = attr.Value;
            if (attr.Origin?.ToString() == oldValue?.ToString())
                oldValue = attr.Value;
        }
        InternalItems.Add(new ValueDiffItem(propertyInfo)
        {
            OriginValue = oldValue?.ToString(),
            CurrentValue = value?.ToString(),
        });
    }

    public bool Contains(string propertyName)
        => InternalItems.Any(o => o.Property.Name == propertyName);
}

/// <summary>
/// 差异项
/// </summary>
/// <param name="property"></param>
public class ValueDiffItem(PropertyInfo property)
{
    public PropertyInfo Property { get; } = property;

    public string? OriginValue { get; set; }

    public string? CurrentValue { get; set; }
}
