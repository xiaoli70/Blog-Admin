namespace Easy.Admin.Core.Const;

public class CacheConst
{
    /// <summary>
    /// 权限缓存键
    /// </summary>
    public const string PermissionKey = "permission.";

    /// <summary>
    /// 用户菜单按钮权限缓存键(菜单按钮Id集合)
    /// </summary>
    public const string PermissionMenuButtonIdKey = "permission.user.menubtnid.";

    /// <summary>
    /// 用户菜单信息权限缓存键（构建ui左侧菜单）
    /// </summary>
    public const string PermissionMenuKey = "permission.user.menu.";

    /// <summary>
    /// 用户可访问的按钮权限编码缓存键（权限编码集合）
    /// </summary>
    public const string PermissionButtonCodeKey = "permission.user.buttoncode.";

    /// <summary>
    /// 系统用户角色Id缓存键
    /// </summary>
    public const string SysUserRoleIdListKey = "sysuser.roleids.";
}