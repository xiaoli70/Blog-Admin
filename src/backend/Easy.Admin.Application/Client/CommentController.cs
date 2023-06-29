using Easy.Admin.Application.Auth;
using Easy.Admin.Application.Client.Dtos;

namespace Easy.Admin.Application.Client;

/// <summary>
/// 评论
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
public class CommentController : IDynamicApiController
{
    private readonly ISqlSugarRepository<Comments> _repository;
    private readonly AuthManager _authManager;

    public CommentController(ISqlSugarRepository<Comments> repository, AuthManager authManager)
    {
        _repository = repository;
        _authManager = authManager;
    }
    /// <summary>
    /// 评论列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<PageResult<CommentOutput>> Get([FromQuery] CommentPageQueryInput dto)
    {
        long userId = _authManager.UserId;
        return await _repository.AsQueryable().LeftJoin<AuthAccount>((comments, account) => comments.AccountId == account.Id)
              .Where(comments => comments.ModuleId == dto.Id && comments.RootId == null) //排除回复的评论
              .OrderByDescending(comments => comments.Id)
              .Select(comments => new CommentOutput
              {
                  Id = comments.Id,
                  Content = comments.Content,
                  PraiseTotal = SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == comments.Id).Count(),
                  IsPraise = SqlFunc.IIF(userId == 0, false, SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == comments.Id && x.AccountId == userId).Any()),
                  ReplyCount = SqlFunc.Subqueryable<Comments>().Where(x => x.RootId == comments.Id).Count(),
                  IP = comments.IP,
                  Geolocation = comments.Geolocation,
                  CreatedTime = comments.CreatedTime
              }).Mapper(async it =>
              {
                  if (it.ReplyCount > 0)
                  {
                      it.ReplyList = await ReplyList(new CommentPageQueryInput()
                      {
                          PageNo = 1,
                          Id = it.Id
                      });
                  }
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 回复分页
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<PageResult<ReplyOutput>> ReplyList([FromQuery] CommentPageQueryInput dto)
    {
        dto.PageSize = dto.PageSize = 5;
        return await _repository.AsQueryable().LeftJoin<AuthAccount>((c, a1) => c.AccountId == a1.Id)
              .LeftJoin<AuthAccount>((c, a1, a2) => c.ReplyAccountId == a2.Id)
              .Where(c => c.RootId == dto.Id)
              .OrderBy(c => c.Id)
              .Select((c, a1, a2) => new ReplyOutput
              {
                  Id = c.Id,
                  ParentId = c.ParentId,
                  AccountId = c.AccountId,
                  ReplyAccountId = c.ReplyAccountId,
                  NikeName = a1.Name,
                  RelyNikeName = a2.Name,
                  RootId = c.RootId,
                  Avatar = a1.Avatar,
                  IP = c.IP,
                  Geolocation = c.Geolocation,
                  CreatedTime = c.CreatedTime
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 评论、回复
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Comments(AddCommentInput dto)
    {
        var comments = dto.Adapt<Comments>();
        comments.AccountId = _authManager.UserId;
        await _repository.InsertAsync(comments);
    }
}