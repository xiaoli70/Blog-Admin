
using System;
using System.Collections.Generic;
namespace Easy.Admin.Core.Config;
public class BlogSetting
{
    /// <summary>
    /// 网站Logo
    /// </summary>
    public string Logo { get; set; }
    /// <summary>
    /// 站点图标
    /// </summary>
    public string Favicon { get; set; }
    /// <summary>
    /// 站点名称
    /// </summary>
    public string SiteName { get; set; }
    /// <summary>
    /// 备案
    /// </summary>
    public string Filing { get; set; }
    /// <summary>
    /// 站点版权
    /// </summary>
    public string Copyright { get; set; }
    /// <summary>
    /// 关键词
    /// </summary>
    public string Keyword { get; set; }
    /// <summary>
    /// 站点描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 允许留言
    /// </summary>
    public bool? IsAllowMessage { get; set; }
    /// <summary>
    /// 允许评论
    /// </summary>
    public bool? IsAllowComments { get; set; }
    /// <summary>
    /// 公告
    /// </summary>
    public string Announcement { get; set; }
}
