using Easy.Admin.Application.Auth;
using Easy.Admin.Application.Menu.Dtos;
using Microsoft.OpenApi.Extensions;

namespace Easy.Admin.Application.Menu;
/// <summary>
/// 系统菜单管理
/// </summary>
public class SysMenuService : BaseService<SysMenu>
{
    private readonly ISqlSugarRepository<SysMenu> _sysMenuRepository;
    private readonly AuthManager _authManager;

    public SysMenuService(ISqlSugarRepository<SysMenu> sysMenuRepository, AuthManager authManager) : base(sysMenuRepository)
    {
        _sysMenuRepository = sysMenuRepository;
        _authManager = authManager;
    }

    /// <summary>
    /// 菜单列表查询
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [Description("菜单列表查询")]
    [HttpGet]
    public async Task<List<SysMenuPageOutput>> Page([FromQuery] string name)
    {
        var q1 = _sysMenuRepository.AsQueryable()
            .OrderBy(x => x.Sort)
            .OrderByDescending(x => x.Id);
        var q2 = _sysMenuRepository.AsQueryable().InnerJoin<SysRoleMenu>((menu, roleMenu) => menu.Id == roleMenu.MenuId)
            .InnerJoin<SysRole>((menu, roleMenu, role) => roleMenu.RoleId == role.Id)
            .InnerJoin<SysUserRole>((menu, roleMenu, role, userRole) => role.Id == userRole.RoleId)
            .Where((menu, roleMenu, role) => role.Status == AvailabilityStatus.Enable);
        if (!string.IsNullOrWhiteSpace(name))
        {
            var list = _authManager.IsSuperAdmin ? await q1.Where(x => x.Name.Contains(name)).ToListAsync() : await q2.Where(menu => menu.Name.Contains(name)).Distinct().ToListAsync();
            return list.Adapt<List<SysMenuPageOutput>>();
        }
        var menus = _authManager.IsSuperAdmin ? await q1.Distinct().ToTreeAsync(x => x.Children, x => x.ParentId, null) : await q2.ToTreeAsync(
               menu => menu.Children, menu => menu.ParentId, null);
        return menus.Adapt<List<SysMenuPageOutput>>();
    }

    /// <summary>
    /// 添加菜单/按钮
    /// </summary>
    /// <returns></returns>
    [Description("添加菜单/按钮")]
    [HttpPost("add")]
    public async Task AddMenu(AddSysMenuInput dto)
    {
        SysMenu sysMenu = dto.Adapt<SysMenu>();
        if (sysMenu.Type == MenuType.Button)
        {
            sysMenu.Link = sysMenu.Icon = sysMenu.Component = sysMenu.Path = sysMenu.Redirect = sysMenu.RouteName = null;
        }
        else
        {
            sysMenu.Code = null;
        }
        await _sysMenuRepository.InsertAsync(sysMenu);
    }

    /// <summary>
    /// 修改菜单/按钮
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("更新菜单/按钮")]
    [HttpPut("edit")]
    public async Task UpdateMenu(UpdateSysMenuInput dto)
    {
        SysMenu sysMenu = await _sysMenuRepository.GetByIdAsync(dto.Id);
        if (sysMenu == null)
        {
            throw Oops.Bah("无线参数");
        }

        //检查菜单父子关系是否存在循环引用
        if (dto.ParentId.HasValue && dto.ParentId != sysMenu.ParentId)
        {
            List<SysMenu> list = await _sysMenuRepository.AsQueryable().ToChildListAsync(x => x.Children, dto.Id);
            if (list.Any(x => x.Id == dto.ParentId))
            {
                throw Oops.Bah($"请勿将当前{dto.Type.GetDisplayName()}的父级菜单设置为它的子级");
            }
        }

        dto.Adapt(sysMenu);
        if (sysMenu.Type == MenuType.Button)
        {
            sysMenu.Link = sysMenu.Icon = sysMenu.Component = sysMenu.Path = sysMenu.Redirect = sysMenu.RouteName = null;
        }
        else
        {
            sysMenu.Code = null;
        }

        await _sysMenuRepository.UpdateAsync(sysMenu);
    }
}