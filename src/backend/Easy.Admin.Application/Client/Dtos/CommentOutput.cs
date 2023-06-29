namespace Easy.Admin.Application.Client.Dtos;

public class CommentOutput
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 评论人ID
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 楼层
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 回复条数
    /// </summary>
    public int ReplyCount { get; set; }
    /// <summary>
    /// 点赞数量
    /// </summary>
    public int PraiseTotal { get; set; }
    /// <summary>
    /// 是否已点赞
    /// </summary>
    public bool IsPraise { get; set; }
    /// <summary>
    /// Ip地址
    /// </summary>
    public string IP { get; set; }
    /// <summary>
    /// Ip归属地
    /// </summary>
    public string Geolocation { get; set; }
    /// <summary>
    /// 评论时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    public PageResult<ReplyOutput> ReplyList { get; set; }
}