using Easy.Admin.Application.Config.Dtos;

namespace Easy.Admin.Application.Config;

/// <summary>
/// 自定义配置项
/// </summary>
public class CustomConfigItemService : BaseService<CustomConfigItem>
{
    private readonly ISqlSugarRepository<CustomConfigItem> _repository;

    public CustomConfigItemService(ISqlSugarRepository<CustomConfigItem> repository) : base(repository)
    {
        _repository = repository;
    }

    //public async Task<> Page()
    //{

    //}

    /// <summary>
    /// 添加自定义配置子项
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("添加自定义配置子项")]
    [HttpPost("add")]
    public async Task AddItem(AddCustomConfigItemInput dto)
    {
        var item = dto.Adapt<CustomConfigItem>();
        await _repository.InsertAsync(item);
    }

    /// <summary>
    /// 修改自定义配置项
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [Description("修改自定义配置项")]
    [HttpPut("edit")]
    public async Task UpdateItem(UpdateCustomConfigItemInput dto)
    {
        var item = await _repository.GetByIdAsync(dto.Id);
        if (item == null) throw Oops.Bah("无效参数");
        dto.Adapt(item);
        await _repository.UpdateAsync(item);
    }
}

public class CustomConfigItemQueryInput : Pagination
{

}