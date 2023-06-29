using MrHuo.OAuth.QQ;

namespace Easy.Admin.Application.Client;
/// <summary>
/// 第三方授权登录
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
[AllowAnonymous]
public class OAuthController : IDynamicApiController
{
    /// <summary>
    /// 第三方授权缓存
    /// </summary>
    private const string OAuthKey = "oauth.";
    private readonly QQOAuth _qqoAuth;
    private readonly ISqlSugarRepository<Account> _accountRepository;
    private readonly IEasyCachingProvider _easyCachingProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdGenerator _idGenerator;

    public OAuthController(QQOAuth qqoAuth,
        ISqlSugarRepository<Account> accountRepository,
        IEasyCachingProvider easyCachingProvider,
        IHttpContextAccessor httpContextAccessor,
        IIdGenerator idGenerator)
    {
        _qqoAuth = qqoAuth;
        _accountRepository = accountRepository;
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
    public async Task<IActionResult> Callback(string type, [FromQuery] string code, [FromQuery] string state)
    {
        Account account = null;
        string encode = _idGenerator.Encode(_idGenerator.NewLong());
        switch (type.ToLower())
        {
            case "qq":
                try
                {
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
                        await _accountRepository.UpdateAsync(x => new Account()
                        {
                            Avatar = info.Avatar,
                            Name = info.Name,
                            Gender = gender
                        },
                            x => x.OAuthId == openId && SqlFunc.ToLower(x.Type) == "qq");
                    }
                    else
                    {
                        account = await _accountRepository.InsertReturnEntityAsync(new Account()
                        {
                            Gender = gender,
                            Avatar = info.Avatar,
                            Name = info.Name,
                            OAuthId = openId,
                            Type = "QQ"
                        });
                    }
                }
                catch (Exception e)
                {

                }
                break;

            default:
                throw Oops.Bah("无效请求");
        }

        string key = $"{OAuthKey}{encode}";
        await _easyCachingProvider.SetAsync(key, account ?? new Account(), TimeSpan.FromSeconds(30));
        string url = App.Configuration["oauth:redirect_uri"];
        return new RedirectResult($"{url}/{encode}");
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpPost("{code}")]
    public async Task Login(string code)
    {
        string key = $"{OAuthKey}{code}";
        var value = await _easyCachingProvider.GetAsync<Account>(key);
        if (value.HasValue)
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
}