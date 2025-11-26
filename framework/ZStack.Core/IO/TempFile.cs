namespace ZStack.Core.IO;

public class TempFile : IDisposable
{
    public string FilePath { get; }

    public TempFile()
    {
        FilePath = Path.GetTempFileName();
    }

    public TempFile(string content)
    {
        FilePath = Path.GetTempFileName();
        File.WriteAllText(FilePath, content);
    }


    public TempFile(string[] content)
    {
        FilePath = Path.GetTempFileName();
        File.WriteAllLines(FilePath, content);
    }

    public TempFile(byte[] content)
    {
        FilePath = Path.GetTempFileName();
        File.WriteAllBytesAsync(FilePath, content);
    }

    public void Dispose()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
        GC.SuppressFinalize(this);
    }
}
