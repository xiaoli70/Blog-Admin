using Easy.Admin.Application.Config.Dtos;

namespace Easy.Admin.Application.Config;

/// <summary>
/// 自定义配置业务
/// </summary>
public class CustomConfigService : IDynamicApiController
{
    private readonly ISqlSugarRepository<CustomConfig> _customConfigRepository;

    public CustomConfigService(ISqlSugarRepository<CustomConfig> customConfigRepository)
    {
        _customConfigRepository = customConfigRepository;
    }

    /// <summary>
    /// 添加配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
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
    /// 编辑配置
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
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
}