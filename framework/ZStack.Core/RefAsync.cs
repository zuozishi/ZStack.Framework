namespace ZStack.Core;

public class RefAsync<T>
{
    public T Value { get; set; }

    public RefAsync(T value)
    {
        Value = value;
    }

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
