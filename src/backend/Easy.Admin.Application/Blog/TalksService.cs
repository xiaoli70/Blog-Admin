namespace Easy.Admin.Application.Blog;

/// <summary>
/// 说说管理
/// </summary>
public class TalksService : BaseService<Talks>
{
    private readonly ISqlSugarRepository<Talks> _repository;

    public TalksService(ISqlSugarRepository<Talks> repository) : base(repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 说说分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("说说分页查询")]
    [HttpGet]
    public async Task<PageResult<TalksPageOutput>> Page([FromQuery] TalksPageQueryInput dto)
    {
        return await _repository.AsQueryable()
              .WhereIF(!string.IsNullOrWhiteSpace(dto.Keyword), x => x.Content.Contains(dto.Keyword))
              .OrderByDescending(x => x.Id)
              .Select(x => new TalksPageOutput
              {
                  Id = x.Id,
                  Status = x.Status,
                  Content = x.Content,
                  IsAllowComments = x.IsAllowComments,
                  IsTop = x.IsTop,
                  CreatedTime = x.CreatedTime
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 添加说说
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("添加说说")]
    [HttpPost("add")]
    public async Task Add(AddTalksInput dto)
    {
        var talks = dto.Adapt<Talks>();
        await _repository.InsertAsync(talks);
    }

    /// <summary>
    /// 更新说说
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("更新说说")]
    [HttpPut("edit")]
    public async Task Update(UpdateTalksInput dto)
    {
        var talks = await _repository.GetByIdAsync(dto.Id);
        dto.Adapt(talks);
        await _repository.UpdateAsync(talks);
    }
}

public class TalksPageOutput
{
    public long Id { get; set; }
    public AvailabilityStatus Status { get; set; }
    public string Content { get; set; }
    public bool IsAllowComments { get; set; }
    public bool IsTop { get; set; }
    public DateTime CreatedTime { get; set; }
}

public class TalksPageQueryInput : Pagination
{
    /// <summary>
    /// 关键词
    /// </summary>
    public string Keyword { get; set; }
}

public class UpdateTalksInput : AddTalksInput
{
    /// <summary>
    /// 说说主键
    /// </summary>
    [Required(ErrorMessage = "缺少必要参数")]
    public long Id { get; set; }
}
public class AddTalksInput
{
    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 说说正文
    /// </summary>
    [Required(ErrorMessage = "说说正文为必填项")]
    public string Content { get; set; }

    /// <summary>
    /// 是否允许评论
    /// </summary>
    public bool IsAllowComments { get; set; }

    /// <summary>
    /// 可用状态
    /// </summary>
    public AvailabilityStatus Status { get; set; }
}