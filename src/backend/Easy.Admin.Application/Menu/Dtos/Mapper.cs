namespace Easy.Admin.Application.Menu.Dtos;

public class Mapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SysMenu, TreeSelectOutput>()
            .Map(dest => dest.Label, src => src.Name)
            .Map(dest => dest.Value, src => src.Id);
    }
}