using Furion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using System.Text;
using Easy.Admin.Core.Options;
using Easy.Core;
using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;

namespace Easy.Admin.Web.Core;

public class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //解决Encoding无法使用gb2312编码问题
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
        services.AddConsoleFormatter();

        //允许跨域
        services.AddCorsAccessor();

        //配置缓存 文档：https://easycaching.readthedocs.io/en/latest/
        services.AddEasyCaching(options =>
        {
            //options.UseCSRedis(App.Configuration);
            //options.WithJson("DefaultCSRedis");
            options.UseInMemory(App.Configuration);
            options.WithJson("DefaultInMemory");
        });

        // 配置ORM 文档：https://www.donet5.com/Home/Doc
        services.AddSqlSugar();

        //远程请求 文档：https://furion.baiqian.ltd/docs/http
        services.AddRemoteRequest();

        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 首字母小写（驼峰样式）
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;//忽略空值
            })
                .AddInjectWithUnifyResult();

        //雪花id 文档：https://github.com/yitter/IdGenerator
        services.AddIdGenerator(App.GetOptions<SnowIdOptions>());

        //注册事件总线 文档：https://furion.baiqian.ltd/docs/event-bus
        services.AddEventBus();

        #region 图形验证码

        //图形验证码
        services.AddCaptcha(App.Configuration, option =>
        {
            option.CaptchaType = CaptchaType.WORD_NUMBER_LOWER; // 验证码类型
            option.CodeLength = 4; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
            option.ExpirySeconds = 60; // 验证码过期时间
            option.IgnoreCase = true; // 比较时是否忽略大小写
            option.StoreageKeyPrefix = ""; // 存储键前缀

            option.ImageOption.Animation = true; // 是否启用动画
            option.ImageOption.FrameDelay = 300; // 每帧延迟,Animation=true时有效, 默认30

            option.ImageOption.Width = 132; // 验证码宽度
            option.ImageOption.Height = 40; // 验证码高度
            //option.ImageOption.BackgroundColor = SixLabors.ImageSharp.Color.White; // 验证码背景色

            option.ImageOption.BubbleCount = 2; // 气泡数量
            option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
            option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
            option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

            option.ImageOption.InterferenceLineCount = 2; // 干扰线数量

            option.ImageOption.FontSize = 36; // 字体大小
            option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体

            /* 
             * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
             * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
             */

            option.ImageOption.TextBold = true;// 粗体，该配置2.0.3新增
        });

        #endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // 添加状态码拦截中间件
        app.UseUnifyResultStatusCodes();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCorsAccessor();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseInject(string.Empty);

        //解决全局异常拦截器中无法拿到请求的内容
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next.Invoke();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
