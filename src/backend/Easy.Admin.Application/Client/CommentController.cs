using Easy.Admin.Application.Auth;
using Easy.Admin.Core.Entities;

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

    public async Task Comments()
    {

    }
}

public class AddCommentInput
{
    /// <summary>
    ///  对应模块ID（null表留言，0代表友链的评论）
    /// </summary>
    public long? ModuleId { get; set; }

    /// <summary>
    /// 顶级楼层评论ID
    /// </summary>
    public long? RootId { get; set; }

    /// <summary>
    /// 被回复的评论ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 当前评论人ID
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 回复人ID
    /// </summary>
    public long? ReplyAccountId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [MaxLength(600, ErrorMessage = "内容限制600个字符内")]
    [Required(ErrorMessage = "请输入内容")]
    public string Content { get; set; }
}

public class ReplyOutput
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 顶级楼层评论ID
    /// </summary>
    public long? RootId { get; set; }
    /// <summary>
    /// 上级评论ID
    /// </summary>
    public long? ParentId { get; set; }
    /// <summary>
    /// 当前评论人ID
    /// </summary>
    public long AccountId { get; set; }
    /// <summary>
    /// 回复人ID
    /// </summary>
    public long? ReplyAccountId { get; set; }
    /// <summary>
    /// 当前人昵称
    /// </summary>
    public string NikeName { get; set; }
    /// <summary>
    /// 回复人昵称
    /// </summary>
    public string RelyNikeName { get; set; }
    /// <summary>
    /// 当前评论人头像
    /// </summary>
    public string Avatar { get; set; }
    /// <summary>
    /// Ip地址
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// Ip所属地
    /// </summary>
    public string Geolocation { get; set; }
    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CreatedTime { get; set; }
}

public class CommentOutput
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 评论人ID
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 楼层
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 回复条数
    /// </summary>
    public int ReplyCount { get; set; }
    /// <summary>
    /// 点赞数量
    /// </summary>
    public int PraiseTotal { get; set; }
    /// <summary>
    /// 是否已点赞
    /// </summary>
    public bool IsPraise { get; set; }
    /// <summary>
    /// Ip地址
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// Ip归属地
    /// </summary>
    public string Geolocation { get; set; }
    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    public PageResult<ReplyOutput> ReplyList { get; set; }
}

public class CommentPageQueryInput : Pagination
{
    /// <summary>
    /// 对应模块ID或评论ID（null表留言，0代表友链的评论）
    /// </summary>
    public long? Id { get; set; }
}