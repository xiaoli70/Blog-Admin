using Easy.Admin.Application.Client.Dtos;

namespace Easy.Admin.Application.Client;

/// <summary>
/// 博客说说
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
[AllowAnonymous]
public class TalksController : IDynamicApiController
{
    private readonly ISqlSugarRepository<Talks> _talksRepository;

    public TalksController(ISqlSugarRepository<Talks> talksRepository)
    {
        _talksRepository = talksRepository;
    }

    /// <summary>
    /// 说说列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<TalksOutput>> Get([FromQuery] Pagination dto)
    {
        return await _talksRepository.AsQueryable().Where(x => x.Status == AvailabilityStatus.Enable)
              .OrderByDescending(x => x.IsTop)
              .OrderBy(x => x.Id)
              .Select(x => new TalksOutput
              {
                  Id = x.Id,
                  IsTop = x.IsTop,
                  Content = x.Content,
                  Upvote = 0,
                  Comments = 0,
                  CreatedTime = x.CreatedTime
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 说说详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<TalkDetailOutput> TalkDetail([FromQuery] long id)
    {
        return await _talksRepository.AsQueryable().Select(x => new TalkDetailOutput
        {
            Id = x.Id,
            Content = x.Content,
            IsTop = x.IsTop,
            IsAllowComments = x.IsAllowComments,
            CreatedTime = x.CreatedTime
        }).FirstAsync();
    }
}