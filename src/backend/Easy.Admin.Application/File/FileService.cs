using Easy.Admin.Core.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using OnceMi.AspNetCore.OSS;

namespace Easy.Admin.Application.File;

public class FileService : IDynamicApiController, ITransient
{
    private readonly IOSSService _ossService;
    private readonly IOptionsMonitor<OssConnectionOptions> _ossOptionsMonitor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IIdGenerator _idGenerator;

    public FileService(IOSSService ossService,
        IOptionsMonitor<OssConnectionOptions> ossOptionsMonitor,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IIdGenerator idGenerator)
    {
        _ossService = ossService;
        _ossOptionsMonitor = ossOptionsMonitor;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _idGenerator = idGenerator;
    }

    /// <summary>
    /// 上传附件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [NonUnify]
    public async Task<List<UploadFileOutput>> Upload([Required] IFormFile file)
    {
        var cookies = _httpContextAccessor.HttpContext!.Request.Cookies;
        if (!cookies.Any() || !cookies.ContainsKey("access-token"))
        {
            throw Oops.Bah("未授权");
        }

        //读取cookie中的token，校验是否登录
        string token = "Bearer " + cookies["access-token"]?.Trim('"');
        _httpContextAccessor.HttpContext!.Request.Headers.Add(new KeyValuePair<string, StringValues>("Authorization", $"{token}"));
        if (!JWTEncryption.ValidateJwtBearerToken(_httpContextAccessor.HttpContext as DefaultHttpContext, out JsonWebToken t))
        {
            throw Oops.Bah(401);
        }
        if (file is null or { Length: 0 })
        {
            throw Oops.Oh("请上传文件");
        }
        var options = _ossOptionsMonitor.CurrentValue;
        var now = DateTime.Today;
        string name = _idGenerator.Encode(_idGenerator.NewLong());
        string extension = Path.GetExtension(file.FileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            throw Oops.Bah("无效文件");
        }
        //文件路径
        string filePath = $"/{now.Year}/{now.Month:D2}/{now.Day:D2}/";
        // 文件完整名称
        if (!options.Enable)
        {
            filePath = string.Concat(options.Bucket.TrimEnd('/'), filePath);//ptions.Bucket.TrimEnd('/') + filePath;
            string s = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
            if (!Directory.Exists(s))
            {
                Directory.CreateDirectory(s);
            }

            var stream = System.IO.File.Create($"{s}{name}{extension}");
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
            var request = _httpContextAccessor.HttpContext!.Request;
            string url = $"{request.Scheme}://{request.Host.Value}/{filePath}{name}{extension}";
            return new List<UploadFileOutput>()
            {
                new()
                {
                    Name = $"{name}{extension}",
                    Url = url
                }
            };
        }
        string fileName = $"/{filePath}/{name}{extension}";
        await _ossService.PutObjectAsync(options.Bucket, fileName, file.OpenReadStream());
        return new List<UploadFileOutput>()
        {
            new()
            {
                Name = $"{name}{extension}",
                Url = $"{options.Domain.TrimEnd('/')}{options.Bucket}{fileName}"
            }
        };
    }
}

public class UploadFileOutput
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 附件链接
    /// </summary>
    public string Url { get; set; }
}