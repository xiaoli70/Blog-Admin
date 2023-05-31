using Furion.ConfigurableOptions;
using OnceMi.AspNetCore.OSS;

namespace Easy.Admin.Core.Options;

public class OssOptions : OSSOptions, IConfigurableOptions
{
    /// <summary>
    /// 默认存目录
    /// </summary>
    public string Bucket { get; set; }
}