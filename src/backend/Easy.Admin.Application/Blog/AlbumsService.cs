namespace Easy.Admin.Application.Blog;
/// <summary>
/// 相册管理
/// </summary>
public class AlbumsService : BaseService<Albums>
{
    private readonly ISqlSugarRepository<Albums> _repository;

    public AlbumsService(ISqlSugarRepository<Albums> repository) : base(repository)
    {
        _repository = repository;
    }

    [Description("相册列表分页查询")]
    [HttpGet]
    public async Task<PageResult<AlbumsPageOutput>> Page([FromQuery] AlbumsPageQueryInput dto)
    {
        return await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .OrderBy(x => x.Sort)
            .Select(x => new AlbumsPageOutput
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Status = x.Status,
                IsVisible = x.IsVisible,
                Sort = x.Sort,
                Remark = x.Remark,
                Cover = x.Cover,
                CreatedTime = x.CreatedTime
            }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 新增相册
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("新增相册")]
    [HttpPost("add")]
    public async Task Add(AddAlbumsInput dto)
    {
        var albums = dto.Adapt<Albums>();
        await _repository.InsertAsync(albums);
    }

    /// <summary>
    /// 更新相册
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("更新相册信息")]
    [HttpPut("edit")]
    public async Task Update(UpdateAlbumsInput dto)
    {
        var albums = await _repository.GetByIdAsync(dto.Id);
        if (albums == null) throw Oops.Bah("无效参数");
        dto.Adapt(albums);
        await _repository.UpdateAsync(albums);
    }


}

public class AlbumsPageOutput
{
    /// <summary>
    /// 相册ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 相册类型
    /// </summary>
    public CoverType? Type { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IsVisible { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    /// 封面
    /// </summary>
    public string Cover { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

public class AlbumsPageQueryInput : Pagination
{
    /// <summary>
    /// 相册名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 相册类型
    /// </summary>
    public CoverType? Type { get; set; }
}

public class UpdateAlbumsInput : AddAlbumsInput
{
    [Required(ErrorMessage = "缺少必要参数")]
    public long Id { get; set; }
}
public class AddAlbumsInput
{
    /// <summary>
    /// 相册名称
    /// </summary>
    [Required(ErrorMessage = "相册名称为必填项")]
    [MaxLength(32, ErrorMessage = "相册名称限制32个字符")]
    public string Name { get; set; }

    /// <summary>
    /// 封面图
    /// </summary>
    [Required(ErrorMessage = "请上传相册封面")]
    [MaxLength(256)]
    public string Cover { get; set; }

    /// <summary>
    /// 相册类型
    /// </summary>
    public CoverType? Type { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }

    /// <summary>
    /// 排序值（值越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序值为必填项")]
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "备注限制200个字符内")]
    public string Remark { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible { get; set; }
}