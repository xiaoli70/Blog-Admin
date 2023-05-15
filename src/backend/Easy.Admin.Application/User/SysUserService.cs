using Easy.Admin.Application.Auth;
using Easy.Admin.Application.User.Dtos;

namespace Easy.Admin.Application.User;

/// <summary>
/// 系统用户管理
/// </summary>
public class SysUserService : BaseService<SysUser>, ITransient
{
    private readonly ISqlSugarRepository<SysUser> _repository;
    private readonly ISqlSugarRepository<SysUserRole> _userRoleRepository;
    private readonly ISqlSugarRepository<SysOrganization> _orgRepository;
    private readonly AuthManager _authManager;
    private readonly IEasyCachingProvider _easyCachingProvider;
    private readonly IIdGenerator _idGenerator;

    public SysUserService(ISqlSugarRepository<SysUser> repository,
        ISqlSugarRepository<SysUserRole> userRoleRepository,
        ISqlSugarRepository<SysOrganization> orgRepository,
        AuthManager authManager,
        IEasyCachingProvider easyCachingProvider,
        IIdGenerator idGenerator) : base(repository)
    {
        _repository = repository;
        _userRoleRepository = userRoleRepository;
        _orgRepository = orgRepository;
        _authManager = authManager;
        _easyCachingProvider = easyCachingProvider;
        _idGenerator = idGenerator;
    }

    /// <summary>
    /// 系统用户分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<SysUserPageOutput>> Page([FromQuery] QueryUserInput dto)
    {
        List<long> orgIdList = new List<long>();
        if (dto.OrgId.HasValue)
        {
            orgIdList.Add(dto.OrgId.Value);
            var list = await _orgRepository.AsQueryable().ToChildListAsync(x => x.Children, dto.OrgId);
            orgIdList.AddRange(list.Select(x => x.Id));
        }

        return await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Account), x => x.Account.Contains(dto.Account))
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Mobile), x => x.Mobile.Contains(dto.Mobile))
            .WhereIF(orgIdList.Any(), x => orgIdList.Contains(x.OrgId))
            .Select(x => new SysUserPageOutput
            {
                Name = x.Name,
                Status = x.Status,
                Account = x.Account,
                Birthday = x.Birthday,
                Mobile = x.Mobile,
                Gender = x.Gender,
                NickName = x.NickName,
                CreatedTime = x.CreatedTime,
                Email = x.Email
            }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 添加系统用户
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [UnitOfWork, HttpPost("add")]
    [Description("添加系统用户")]
    public async Task AddUser(AddUserInput dto)
    {
        var user = dto.Adapt<SysUser>();
        user.Id = _idGenerator.NextId();
        string encode = _idGenerator.Encode(user.Id);
        user.Password = MD5Encryption.Encrypt(encode + "123456");
        var roles = dto.Roles.Select(x => new SysUserRole()
        {
            RoleId = x,
            UserId = user.Id
        }).ToList();
        await _repository.InsertAsync(user);
        await _userRoleRepository.InsertRangeAsync(roles);
    }

    /// <summary>
    /// 更新系统用户信息
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("更新系统用户信息")]
    [UnitOfWork, HttpPut("edit")]
    public async Task UpdateUser(UpdateUserInput dto)
    {
        var user = await _repository.GetByIdAsync(dto.Id);
        if (user == null) throw Oops.Bah("无效参数");

        dto.Adapt(user);
        var roles = dto.Roles.Select(x => new SysUserRole()
        {
            RoleId = x,
            UserId = user.Id
        }).ToList();
        await _repository.UpdateAsync(user);
        await _userRoleRepository.InsertRangeAsync(roles);
    }
    /// <summary>
    /// 重置密码
    /// </summary>
    /// <returns></returns>
    [Description("重置密码")]
    public async Task Reset(ResetPasswordInput dto)
    {
        string encrypt = MD5Encryption.Encrypt(_idGenerator.Encode(dto.Id) + dto.Password);
        await _repository.UpdateAsync(x => new SysUser()
        {
            Password = encrypt
        }, x => x.Id == dto.Id);
    }
}