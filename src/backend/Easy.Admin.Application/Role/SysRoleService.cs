using Easy.Admin.Application.Role.Dtos;

namespace Easy.Admin.Application.Role;

/// <summary>
/// 角色管理
/// </summary>
public class SysRoleService : BaseService<SysRole>
{
    private readonly ISqlSugarRepository<SysRole> _sysRoleRepository;
    private readonly ISqlSugarRepository<SysRoleMenu> _sysRoleMenuRepository;
    private readonly IEasyCachingProvider _easyCachingProvider;
    private readonly IIdGenerator _idGenerator;

    public SysRoleService(ISqlSugarRepository<SysRole> sysRoleRepository,
        ISqlSugarRepository<SysRoleMenu> sysRoleMenuRepository,
        IEasyCachingProvider easyCachingProvider,
        IIdGenerator idGenerator) : base(sysRoleRepository)
    {
        _sysRoleRepository = sysRoleRepository;
        _sysRoleMenuRepository = sysRoleMenuRepository;
        _easyCachingProvider = easyCachingProvider;
        _idGenerator = idGenerator;
    }

    /// <summary>
    /// 角色分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<SysRolePageOutput>> Page([FromQuery] SysRoleQueryInput dto)
    {
        return await _sysRoleRepository.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .OrderBy(x => x.Sort)
            .OrderByDescending(x => x.Id)
            .Select(x => new SysRolePageOutput
            {
                Id = x.Id,
                Name = x.Name,
                CreatedTime = x.CreatedTime,
                Status = x.Status,
                Code = x.Code,
                Sort = x.Sort,
                Remark = x.Remark
            }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 添加角色
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("添加角色")]
    [HttpPost("add")]
    [UnitOfWork]
    public async Task AddRole(AddSysRoleInput dto)
    {
        if (await _sysRoleRepository.IsAnyAsync(x => x.Code == dto.Code))
        {
            throw Oops.Bah("角色编码已存在");
        }

        var role = dto.Adapt<SysRole>();
        role.Id = _idGenerator.NewLong();
        var roleMenus = dto.Menus.Select(x => new SysRoleMenu()
        {
            MenuId = x,
            RoleId = role.Id
        }).ToList();
        await _sysRoleRepository.InsertAsync(role);
        await _sysRoleMenuRepository.InsertRangeAsync(roleMenus);
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("更新角色")]
    [HttpPut("edit")]
    [UnitOfWork]
    public async Task UpdateRole(UpdateSysRoleInput dto)
    {
        var sysRole = await _sysRoleRepository.GetByIdAsync(dto.Id);
        if (sysRole == null)
        {
            throw Oops.Bah("无效参数");
        }
        if (await _sysRoleRepository.IsAnyAsync(x => x.Id != dto.Id && x.Code == dto.Code))
        {
            throw Oops.Bah("角色编码已存在");
        }

        dto.Adapt(sysRole);
        var roleMenus = dto.Menus.Select(x => new SysRoleMenu()
        {
            MenuId = x,
            RoleId = sysRole.Id
        }).ToList();
        await _sysRoleRepository.UpdateAsync(sysRole);
        await _sysRoleMenuRepository.DeleteAsync(x => x.RoleId == sysRole.Id);
        await _sysRoleMenuRepository.InsertRangeAsync(roleMenus);
        await _easyCachingProvider.RemoveByPrefixAsync(CacheConst.PermissionKey);
    }


    /// <summary>
    /// 修改角色状态
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("修改角色状态"), HttpPut("setStatus")]
    public override async Task SetStatus(AvailabilityDto dto)
    {
        await base.SetStatus(dto);
        await _easyCachingProvider.RemoveByPrefixAsync(CacheConst.PermissionKey);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("删除角色"), HttpDelete("delete")]
    public override async Task Delete(KeyDto dto)
    {
        await base.Delete(dto);
        await _easyCachingProvider.RemoveByPrefixAsync(CacheConst.PermissionKey);
    }
}