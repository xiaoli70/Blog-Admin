using Easy.Admin.Application.Auth;
using Easy.Admin.Application.Client.Dtos;
using Easy.Admin.Core.Entities;

namespace Easy.Admin.Application.Client;

/// <summary>
/// 评论
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
public class CommentController : IDynamicApiController
{
    private readonly ISqlSugarRepository<Comments> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthManager _authManager;

    public CommentController(ISqlSugarRepository<Comments> repository,
        IHttpContextAccessor httpContextAccessor, AuthManager authManager)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
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
              .Select((comments, account) => new CommentOutput
              {
                  Id = comments.Id,
                  Content = comments.Content,
                  PraiseTotal = SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == comments.Id).Count(),
                  IsPraise = SqlFunc.IF(userId == 0).Return(false).End(SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == comments.Id && x.AccountId == userId).Any()),
                  ReplyCount = SqlFunc.Subqueryable<Comments>().Where(x => x.RootId == comments.Id).Count(),
                  IP = comments.IP,
                  AccountId = account.Id,
                  NickName = account.Name,
                  IsBlogger = account.IsBlogger,
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
        long userId = _authManager.UserId;
        dto.PageSize = dto.PageSize = 5;
        return await _repository.AsQueryable().LeftJoin<AuthAccount>((c, a1) => c.AccountId == a1.Id)
              .LeftJoin<AuthAccount>((c, a1, a2) => c.ReplyAccountId == a2.Id)
              .Where(c => c.RootId == dto.Id)
              .OrderBy(c => c.Id)
              .Select((c, a1, a2) => new ReplyOutput
              {
                  Id = c.Id,
                  Content = c.Content,
                  ParentId = c.ParentId,
                  AccountId = c.AccountId,
                  ReplyAccountId = c.ReplyAccountId,
                  IsBlogger = a1.IsBlogger,
                  NikeName = a1.Name,
                  RelyNikeName = a2.Name,
                  RootId = c.RootId,
                  Avatar = a1.Avatar,
                  PraiseTotal = SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == c.Id).Count(),
                  IsPraise = SqlFunc.IF(userId == 0).Return(false).End(SqlFunc.Subqueryable<Praise>().Where(x => x.ObjectId == c.Id && x.AccountId == userId).Any()),
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
    public async Task Add(AddCommentInput dto)
    {
        string address = _httpContextAccessor.HttpContext.GetGeolocation();
        var comments = dto.Adapt<Comments>();
        comments.AccountId = _authManager.UserId;
        comments.IP = _httpContextAccessor.HttpContext.GetRemoteIpAddressToIPv4();
        comments.Geolocation = address;
        await _repository.InsertAsync(comments);
    }
}