namespace ZStack.Core;

public class RefAsync<T>(T value)
{
    public T Value { get; set; } = value;

    public override string ToString()
    {
        T value = Value;
        return value?.ToString() ?? string.Empty;
    }

    public static implicit operator T(RefAsync<T> r)
    {
        return r.Value;
    }

    public static implicit operator RefAsync<T>(T value)
    {
        return new RefAsync<T>(value);
    }
}
