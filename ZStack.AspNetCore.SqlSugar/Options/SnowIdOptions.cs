using Furion.ConfigurableOptions;
using Yitter.IdGenerator;

namespace ZStack.AspNetCore.SqlSugar.Options;

/// <summary>
/// 雪花Id配置选项
/// </summary>
public sealed class SnowIdOptions : IdGeneratorOptions, IConfigurableOptions
{
}
