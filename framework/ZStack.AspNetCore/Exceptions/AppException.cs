namespace ZStack.AspNetCore.Exceptions;

/// <summary>
/// 应用异常类
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public AppException() : base()
    {
        ErrorCode = 0;
        ErrorMessage = string.Empty;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public AppException(string message) : this(500, message)
    {
        
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    public AppException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
        ErrorMessage = message;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public AppException(int errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
        ErrorMessage = message;
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string ErrorMessage { get; }
}
