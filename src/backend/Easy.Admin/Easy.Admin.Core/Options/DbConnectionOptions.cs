using System.Collections.Generic;
using Furion.ConfigurableOptions;

namespace Easy.Admin.Core.Options;

/// <summary>
/// 数据库选项配置
/// </summary>
public sealed class DbConnectionOptions : IConfigurableOptions
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public List<ConnectionConfig> Connections { get; set; }
}