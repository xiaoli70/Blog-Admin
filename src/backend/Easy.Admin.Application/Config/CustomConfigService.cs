using Easy.Admin.Application.Config.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Easy.Admin.Application.Config;

/// <summary>
/// 自定义配置业务
/// </summary>
public class CustomConfigService : BaseService<CustomConfig>
{
    private readonly ISqlSugarRepository<CustomConfig> _customConfigRepository;

    public CustomConfigService(ISqlSugarRepository<CustomConfig> customConfigRepository) : base(customConfigRepository)
    {
        _customConfigRepository = customConfigRepository;
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