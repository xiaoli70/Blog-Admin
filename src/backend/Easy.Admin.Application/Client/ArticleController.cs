using Easy.Admin.Application.Client.Dtos;

namespace Easy.Admin.Application.Client;
/// <summary>
/// 博客文章
/// </summary>
[ApiDescriptionSettings("博客前端接口")]
[AllowAnonymous]
public class ArticleController : IDynamicApiController
{
    private readonly ISqlSugarRepository<Tags> _tagsRepository;
    private readonly ISqlSugarRepository<Article> _articleRepository;
    private readonly ISqlSugarRepository<Categories> _categoryRepository;

    public ArticleController(ISqlSugarRepository<Tags> tagsRepository,
        ISqlSugarRepository<Article> articleRepository,
        ISqlSugarRepository<Categories> categoryRepository)
    {
        _tagsRepository = tagsRepository;
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// 文章表查询
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageResult<ArticleOutput>> Get([FromQuery] ArticleListQueryInput dto)
    {
        return await _articleRepository.AsQueryable().LeftJoin<ArticleCategory>((article, ac) => article.Id == ac.ArticleId)
              .InnerJoin<Categories>((article, ac, c) => ac.CategoryId == c.Id && c.Status == AvailabilityStatus.Enable)
              .Where(article => article.Status == AvailabilityStatus.Enable && article.PublishTime <= SqlFunc.GetDate())
              .Where(article => article.ExpiredTime == null || SqlFunc.GetDate() < article.ExpiredTime)
              .WhereIF(dto.CategoryId.HasValue, (article, ac) => ac.CategoryId == dto.CategoryId)
              .WhereIF(dto.TagId.HasValue,
                  article => SqlFunc.Subqueryable<Tags>().InnerJoin<ArticleTag>((tags, at) => tags.Id == at.TagId)
                      .Where((tags, at) => tags.Status == AvailabilityStatus.Enable && at.ArticleId == article.Id).Any())
              .OrderByDescending(article => article.IsTop)
              .OrderBy(article => article.Sort)
              .OrderByDescending(article => article.PublishTime)
              .Select((article, ac, c) => new ArticleOutput
              {
                  Id = article.Id,
                  Title = article.Title,
                  CategoryId = c.Id,
                  CategoryName = c.Name,
                  IsTop = article.IsTop,
                  CreationType = article.CreationType,
                  Summary = article.Summary,
                  Cover = article.Cover,
                  PublishTime = article.PublishTime,
                  Tags = SqlFunc.Subqueryable<Tags>().InnerJoin<ArticleTag>((tags, at) => tags.Id == at.TagId)
                      .Where((tags, at) => tags.Status == AvailabilityStatus.Enable && at.ArticleId == article.Id)
                      .ToList(tags => new TagsOutput
                      {
                          Id = tags.Id,
                          Name = tags.Name,
                          Color = tags.Color,
                          Icon = tags.Icon
                      })
              }).ToPagedListAsync(dto);
    }

    /// <summary>
    /// 标签列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<TagsOutput>> Tags()
    {
        return await _tagsRepository.AsQueryable().Where(x => x.Status == AvailabilityStatus.Enable)
              .OrderBy(x => x.Sort)
              .Select(x => new TagsOutput
              {
                  Id = x.Id,
                  Icon = x.Icon,
                  Name = x.Name,
                  Color = x.Color
              }).ToListAsync();
    }

    /// <summary>
    /// 文章栏目分类
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<CategoryOutput>> Categories()
    {
        var queryable = _articleRepository.AsQueryable().Where(a => a.Status == AvailabilityStatus.Enable && a.PublishTime >= SqlFunc.GetDate() && (a.ExpiredTime == null || SqlFunc.GetDate() < a.ExpiredTime));
        return await _categoryRepository.AsQueryable().LeftJoin<ArticleCategory>((c, ac) => c.Id == ac.CategoryId)
              .LeftJoin(queryable, (c, ac, a) => ac.ArticleId == a.Id)
              .Where(c => c.Status == AvailabilityStatus.Enable)
              .WithCache()
              .GroupBy(c => new { c.Id, c.ParentId, c.Name, c.Sort })
              .Select((c, ac, a) => new CategoryOutput
              {
                  Id = c.Id,
                  ParentId = c.ParentId,
                  Sort = c.Sort,
                  Name = c.Name,
                  Total = SqlFunc.AggregateCount(a.Id)
              })
              .MergeTable()
              .OrderBy(x => x.Sort)
              .OrderBy(x => x.ParentId)
              .OrderBy(x => x.Id)
              .ToListAsync();
    }

    /// <summary>
    /// 文章信息统计
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ArticleReportOutput> Report()
    {
        //统计文章数量
        int articleCount = await _articleRepository.AsQueryable()
            .Where(x => x.Status == AvailabilityStatus.Enable && (x.ExpiredTime == null || SqlFunc.GetDate() < x.ExpiredTime))
            .Where(x => x.PublishTime >= SqlFunc.GetDate())
            .CountAsync();

        //标签统计
        int tagCount = await _tagsRepository.AsQueryable().Where(x => x.Status == AvailabilityStatus.Enable).CountAsync();
        //一级栏目统计
        int categoryCount = await _categoryRepository.AsQueryable().Where(x => x.Status == AvailabilityStatus.Enable).CountAsync();

        return new ArticleReportOutput
        {
            ArticleCount = articleCount,
            CategoryCount = categoryCount,
            TagCount = tagCount
        };
    }
}