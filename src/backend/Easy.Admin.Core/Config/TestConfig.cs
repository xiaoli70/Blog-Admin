
using System;
using System.Collections.Generic;
namespace Easy.Admin.Core.Config;
public class TestConfig
{
    /// <summary>
    /// 封面
    /// </summary>
    public string Cover { get; set; }
    /// <summary>
    /// 相册名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 排序值
    /// </summary>
    public int? Sort { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 附件
    /// </summary>
    public List<string> Files { get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    public List<string> Images { get; set; }
    /// <summary>
    /// 出生日期
    /// </summary>
    public DateTime? date70494 { get; set; }
    /// <summary>
    /// 时间范围
    /// </summary>
    public List<DateTime> daterange47693 { get; set; }
    /// <summary>
    /// 爱好1:逛街；2:听歌；3:玩游戏
    /// </summary>
    public List<string> checkbox5946 { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool? switch68042 { get; set; }
}
