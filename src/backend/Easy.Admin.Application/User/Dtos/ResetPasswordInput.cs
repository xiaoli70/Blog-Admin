namespace Easy.Admin.Application.User.Dtos;

public class ResetPasswordInput : KeyDto
{
    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "密码为必填项")]
    [MaxLength(16, ErrorMessage = "密码限制16个字符内")]
    public string Password { get; set; }
}