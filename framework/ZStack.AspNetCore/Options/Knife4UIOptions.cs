namespace ZStack.AspNetCore.Options;

public class Knife4UIOptions : IGeekFan.AspNetCore.Knife4jUI.Knife4UIOptions, IConfigurableOptions
{
    /// <summary>
    /// 是否启用Knife4UI
    /// </summary>
    public bool Enabled { get; set; } = true;

    public Knife4UIOptions()
    {
        Enabled = true;
        RoutePrefix = "kapi";
    }
}
