namespace ZStackWebApi.Services;

/// <summary>
/// 天气预报服务
/// </summary>
[Route("api/weatherforecast")]
public class WeatherforecastService : IDynamicApiController, ISingleton
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// 获取天气预报
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Weatherforecast> GetAsync()
    {
        return Enumerable.Range(1, 5).Select(index => new Weatherforecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }
}

public class Weatherforecast
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// 摄氏度
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// 华氏度
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// 天气概要
    /// </summary>
    public string? Summary { get; set; }
}
