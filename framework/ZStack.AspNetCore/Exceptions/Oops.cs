using System.Net;

namespace ZStack.AspNetCore.Exceptions;

/// <summary>
/// 抛异常静态类
/// </summary>
public static class Oops
{
    /// <summary>
    /// 抛出系统异常
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static AppException Oh(string message)
        => new(500, message);

    /// <summary>
    /// 抛出系统异常
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    public static AppException Oh(string message, Exception innerException)
        => new(500, message, innerException);

    /// <summary>
    /// 抛出业务异常
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static AppException Bah(string message)
        => new(400, message);

    /// <summary>
    /// 抛出业务异常
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    public static AppException Bah(string message, Exception innerException)
        => new(400, message, innerException);

    /// <summary>
    /// 抛异常
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static AppException Throw(int errorCode, string message)
        => new(errorCode, message);

    /// <summary>
    /// 抛异常
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    public static AppException Throw(int errorCode, string message, Exception innerException)
        => new(errorCode, message, innerException);

    /// <summary>
    /// 抛异常
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static AppException Throw(HttpStatusCode statusCode, string message)
        => new((int)statusCode, message);

    /// <summary>
    /// 抛异常
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <returns></returns>
    public static AppException Throw(HttpStatusCode statusCode, string message, Exception innerException)
        => new((int)statusCode, message, innerException);
}
