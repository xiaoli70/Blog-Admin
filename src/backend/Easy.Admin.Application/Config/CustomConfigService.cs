using Easy.Admin.Application.Config.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Furion.ViewEngine;
using Microsoft.AspNetCore.Hosting;

namespace Easy.Admin.Application.Config;

/// <summary>
/// 自定义配置业务
/// </summary>
public class CustomConfigService : BaseService<CustomConfig>
{
    private readonly ISqlSugarRepository<CustomConfig> _customConfigRepository;
    private readonly IViewEngine _viewEngine;
    private readonly IWebHostEnvironment _environment;

    public CustomConfigService(ISqlSugarRepository<CustomConfig> customConfigRepository,
        IViewEngine viewEngine,
        IWebHostEnvironment environment): base(customConfigRepository)
    {
        _customConfigRepository = customConfigRepository;
        _viewEngine = viewEngine;
        _environment = environment;
    }

    /// <summary>
    /// 自定义配置分页查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("自定义配置分页查询")]
    [HttpGet]
    public async Task<PageResult<CustomConfigPageOutput>> Page([FromQuery] CustomConfigQueryInput dto)
    {
        return await _customConfigRepository.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Name), x => x.Name.Contains(dto.Name))
            .WhereIF(!string.IsNullOrWhiteSpace(dto.Code), x => x.Code.Contains(dto.Code))
            .OrderByDescending(x => x.Id)
            .Select(x => new CustomConfigPageOutput
            {
                Id = x.Id,
                Status = x.Status,
                Remark = x.Remark,
                Name = x.Name,
                Code = x.Code,
                IsMultiple = x.IsMultiple,
                IsGenerate = x.IsGenerate,
                CreatedTime = x.CreatedTime
            }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 添加自定义配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("添加自定义配置")]
    [HttpPost("add")]
    public async Task AddConfig(AddCustomConfigInput dto)
    {
        if (await _customConfigRepository.IsAnyAsync(x => x.Code == dto.Code))
        {
            throw Oops.Bah("编码已存在");
        }
        var config = dto.Adapt<CustomConfig>();
        await _customConfigRepository.InsertAsync(config);
    }

    /// <summary>
    /// 修改自定义配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("修改自定义配置")]
    [HttpPut("edit")]
    public async Task UpdateConfig(UpdateCustomConfigInput dto)
    {
        var config = await _customConfigRepository.GetByIdAsync(dto.Id);
        if (config == null)
        {
            throw Oops.Bah("无效参数");
        }
        if (dto.Code != config.Code && await _customConfigRepository.IsAnyAsync(x => x.Code == dto.Code && x.Id != dto.Id))
        {
            throw Oops.Bah("编码已存在");
        }

        dto.Adapt(config);
        await _customConfigRepository.UpdateAsync(config);
    }

    /// <summary>
    /// 获取配置表单设计
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Description("获取配置表单设计")]
    [HttpGet("GetFormJson")]
    public async Task<string> GetFormJson([FromQuery] long id)
    {
        return await _customConfigRepository.AsQueryable()
                .Where(x => x.Id == id)
                .Select(x => x.Json).FirstAsync();
    }

    /// <summary>
    /// 修改配置表单设计
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("修改配置表单设计")]
    [HttpPatch]
    public async Task SetJson(CustomConfigSetJsonInput dto)
    {
        string json = JsonConvert.SerializeObject(dto.Json);
        await _customConfigRepository.UpdateSetColumnsTrueAsync(x => new CustomConfig()
        {
            Json = json
        }, x => x.Id == dto.Id);
    }

    /// <summary>
    /// 生成自定配置类
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("生成自定配置类")]
    [HttpPost]
    public async Task Generate(KeyDto dto)
    {
        var config = await _customConfigRepository.GetByIdAsync(dto.Id);
        if (config == null) throw Oops.Bah("无效参数");
        if (string.IsNullOrWhiteSpace(config.Json)) throw Oops.Bah("请配置设计");
        var controls = ResolveJson(config.Json);
        if (!controls.Any()) throw Oops.Bah("请配置设计");
        await GenerateCode(config.Code, controls);
    }

    /// <summary>
    /// 解析表单设计
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [NonAction]
    public List<CustomControl> ResolveJson(string json)
    {
        string s = "{\"key\":\\d+,\"type\":\"(input|select|date|switch|number|textarea|radio|checkbox|time|time-range|date-range|rate|color|slider|cascader|rich-editor|file-upload|picture-upload)\".*?\"id\".*?}";
        string value = string.Join(",", Regex.Matches(json, s, RegexOptions.IgnoreCase).Select(x => x.Value));
        string temp = $"[{value}]";
        return JsonConvert.DeserializeObject<List<CustomControl>>(temp);
    }

    /// <summary>
    /// 生成类文件
    /// </summary>
    /// <param name="options"></param>
    [NonAction]
    public async Task GenerateCode(string className, List<CustomControl> options)
    {
        if (!options.Any()) return;
        List<string> fields = new List<string>();
        options.ForEach(x =>
        {
            string type = string.Empty;
            switch (x.Type.ToLower())
            {
                case "select":
                case "checkbox":
                case "cascader":
                    if (x.Options.Multiple)
                    {
                        type = nameof(List<string>);
                    }
                    break;
                case "date":
                    type = x.Options.Required ? nameof(DateTime) : "DateTime?";
                    break;
                case "switch":
                    type = x.Options.Required ? "bool" : "bool?";
                    break;
                case "number":
                    type = x.Options.Precision > 0 ? "double" : "int";
                    type += x.Options.Required ? string.Empty : "?";
                    break;
                case "time":
                    type = x.Options.Required ? nameof(TimeOnly) : "TimeOnly?";
                    break;
                case "time-range":
                    type = nameof(List<TimeOnly>);
                    break;
                case "date-range":
                    type = nameof(List<DateTime>);
                    break;
                case "rate":
                    type = x.Options.AllowHalf ? "double" : "int";
                    type += x.Options.Required ? string.Empty : "?";
                    break;
                default:
                    type = "string";
                    break;
            }

            string remark = x.Options.OptionItems?.Any() ?? false
                ? string.Join("；", x.Options.OptionItems.Select(s => $"{s.Value}:{s.Value}"))
                : string.Empty;
            string template = @$"
    /// <summary>
    /// {x.Options.Label}{remark}
    /// </summary>
    public {type} {x.Options.Name} {{ get; set; }}";
            fields.Add(template);
        });

        string template = @"
using System;
using System.Collections.Generic;
public class @Model.ClassName
{@foreach(var item in Model.Items)
    {
        @item
    }

}
";
        string content = await _viewEngine.RunCompileFromCachedAsync(template, new { ClassName = className, Items = fields });
        string path = _environment.WebRootPath;
        string directory = Directory.GetCurrentDirectory();
    }
}

public class CustomConfigSetJsonInput
{
    /// <summary>
    /// 自定义配置ID
    /// </summary>
    [Required(ErrorMessage = "缺少必要参数")]
    public long Id { get; set; }

    /// <summary>
    /// 表单设计
    /// </summary>
    [Required(ErrorMessage = "请设计表单")]
    public JObject Json { get; set; }
}