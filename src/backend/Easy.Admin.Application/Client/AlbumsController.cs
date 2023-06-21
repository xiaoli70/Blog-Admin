using Easy.Admin.Application.Client.Dtos;

namespace Easy.Admin.Application.Client;

/// <summary>
/// 相册
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
[AllowAnonymous]
public class AlbumsController : IDynamicApiController
{
    private readonly ISqlSugarRepository<Albums> _albumsRepository;

    public AlbumsController(ISqlSugarRepository<Albums> albumsRepository)
    {
        _albumsRepository = albumsRepository;
    }

    /// <summary>
    /// 相册列表
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<AlbumsOutput>> Get([FromQuery] Pagination dto)
    {
        return await _albumsRepository.AsQueryable().Where(x => x.IsVisible && x.Status == AvailabilityStatus.Enable)
              .OrderBy(x => x.Sort)
              .OrderByDescending(x => x.Id)
              .Select(x => new AlbumsOutput
              {
                  Id = x.Id,
                  Name = x.Name,
                  Cover = x.Cover,
                  CreatedTime = x.CreatedTime
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 相册下的图片
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<PictureOutput>> Pictures([FromQuery] PicturesQueryInput dto)
    {
        return await _albumsRepository.AsQueryable().InnerJoin<Pictures>((albums, pictures) => albums.Id == pictures.AlbumId)
             .Where(albums => albums.IsVisible && albums.Status == AvailabilityStatus.Enable && albums.Id == dto.AlbumId)
             .OrderByDescending((albums, pictures) => pictures.Id)
             .Select((albums, pictures) => new PictureOutput { Id = pictures.Id, Url = pictures.Url }).ToPagedListAsync(dto);
    }
}