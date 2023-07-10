using Easy.Admin.Application.Auth;
using Easy.Admin.Application.Client.Dtos;
using MrHuo.OAuth.QQ;

namespace Easy.Admin.Application.Client;
/// <summary>
/// 第三方授权登录
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
public class OAuthController : IDynamicApiController
{
    /// <summary>
    /// 第三方授权缓存
    /// </summary>
    private const string OAuthKey = "oauth.";
    private readonly QQOAuth _qqoAuth;
    private readonly AuthManager _authManager;
    private readonly ISqlSugarRepository<AuthAccount> _accountRepository;
    private readonly ISqlSugarRepository<FriendLink> _friendLinkRepository;
    private readonly IEasyCachingProvider _easyCachingProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdGenerator _idGenerator;

    public OAuthController(QQOAuth qqoAuth,
        AuthManager authManager,
        ISqlSugarRepository<AuthAccount> accountRepository,
        ISqlSugarRepository<FriendLink> friendLinkRepository,
        IEasyCachingProvider easyCachingProvider,
        IHttpContextAccessor httpContextAccessor,
        IIdGenerator idGenerator)
    {
        _qqoAuth = qqoAuth;
        _authManager = authManager;
        _accountRepository = accountRepository;
        _friendLinkRepository = friendLinkRepository;
        _easyCachingProvider = easyCachingProvider;
        _httpContextAccessor = httpContextAccessor;
        _idGenerator = idGenerator;
    }

    /// <summary>
    /// 获取授权地址
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpGet("{type}")]
    [AllowAnonymous]
    public string Get(string type)
    {
        return type.ToLower() switch
        {
            "qq" => _qqoAuth.GetAuthorizeUrl(_idGenerator.Encode(_idGenerator.NewLong())),
            _ => throw Oops.Bah("无效请求")
        };
    }

    /// <summary>
    /// 授权回调
    /// </summary>
    /// <param name="type">授权类型</param>
    /// <param name="code"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [HttpGet("{type}/callback")]
    [AllowAnonymous]
    public async Task<IActionResult> Callback(string type, [FromQuery] string code, [FromQuery] string state)
    {
        AuthAccount account;
        string encode = _idGenerator.Encode(_idGenerator.NewLong());
        switch (type.ToLower())
        {
            case "qq":
                var auth = await _qqoAuth.AuthorizeCallback(code, state ?? "");
                if (!auth.IsSccess)
                {
                    throw Oops.Bah(auth.ErrorMessage);
                }
                var info = auth.UserInfo;
                string openId = await _qqoAuth.GetOpenId(auth.AccessToken.AccessToken);
                account = await _accountRepository.AsQueryable().FirstAsync(x => x.OAuthId == openId && SqlFunc.ToLower(x.Type) == "qq");
                var gender = info.Gender == "男" ? Gender.Male :
                    info.Gender == "女" ? Gender.Female : Gender.Unknown;
                if (account != null)
                {
                    await _accountRepository.UpdateAsync(x => new AuthAccount()
                    {
                        Avatar = info.QQAvatar,
                        Name = info.Name,
                        Gender = gender
                    },
                        x => x.OAuthId == openId && SqlFunc.ToLower(x.Type) == "qq");
                }
                else
                {
                    account = await _accountRepository.InsertReturnEntityAsync(new AuthAccount()
                    {
                        Gender = gender,
                        Avatar = info.Avatar,
                        Name = info.Name,
                        OAuthId = openId,
                        Type = "QQ"
                    });
                }
                break;

            default:
                throw Oops.Bah("无效请求");
        }

        string key = $"{OAuthKey}{encode}";
        await _easyCachingProvider.SetAsync(key, account, TimeSpan.FromSeconds(30));
        string url = App.Configuration["oauth:redirect_uri"];
        return new RedirectResult($"{url}/{encode}");
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost("login/{code}")]
    [AllowAnonymous]
    public async Task Login(string code)
    {
        string key = $"{OAuthKey}{code}";
        var value = await _easyCachingProvider.GetAsync<AuthAccount>(key);
        if (!value.HasValue)
        {
            throw Oops.Bah("无效参数");
        }
        long uniqueId = _idGenerator.NewLong();
        var account = value.Value;
        string token = JWTEncryption.Encrypt(new Dictionary<string, object>()
        {
            [AuthClaimsConst.AuthIdKey] = account.Id,
            [AuthClaimsConst.AccountKey] = account.OAuthId,
            [AuthClaimsConst.UuidKey] = uniqueId,
            [AuthClaimsConst.AuthPlatformTypeKey] = AuthPlatformType.Blog
        });
        // 获取刷新 token
        var refreshToken = JWTEncryption.GenerateRefreshToken(token);
        // 设置响应报文头
        _httpContextAccessor.HttpContext!.Response.Headers["access-token"] = token;
        _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = refreshToken;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<OAuthAccountDetailOutput> UserInfo()
    {
        long id = _authManager.UserId;
        return await _accountRepository.AsQueryable().LeftJoin<FriendLink>((account, link) => account.Id == link.AppUserId)
            .Where(account => account.Id == id)
            .Select((account, link) => new OAuthAccountDetailOutput
            {
                Id = account.Id,
                Avatar = account.Avatar,
                Status = link.Status,
                NickName = account.Name,
                Link = link.Link,
                Logo = link.Logo,
                SiteName = link.SiteName,
                Url = link.Url,
                Remark = link.Remark
            }).FirstAsync();
    }

    /// <summary>
    /// 申请友链
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task AddLink(AddLinkOutput dto)
    {
        long userId = _authManager.UserId;

        var link = await _friendLinkRepository.GetFirstAsync(x => x.AppUserId == userId);
        if (link == null)
        {
            link = dto.Adapt<FriendLink>();
            link.AppUserId = userId;
            link.Status = AvailabilityStatus.Disable;
            await _friendLinkRepository.InsertAsync(link);
            return;
        }

        link = dto.Adapt(link);
        link.Status = AvailabilityStatus.Disable;
        await _friendLinkRepository.UpdateAsync(link);
    }
}