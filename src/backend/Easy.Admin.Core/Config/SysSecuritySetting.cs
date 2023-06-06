
using System;
using System.Collections.Generic;
namespace Easy.Admin.Core.Config;
public class SysSecuritySetting
{
    /// <summary>
    /// 用户默认密码
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// 密码错误次数
    /// </summary>
    public int Times { get; set; }
    /// <summary>
    /// 密码规则
    /// </summary>
    public string PasswordRule { get; set; }
    /// <summary>
    /// 密码验证提示语
    /// </summary>
    public string RuleMessage { get; set; }
    /// <summary>
    /// 单点登录
    /// </summary>
    public bool? IsSso { get; set; }
}
