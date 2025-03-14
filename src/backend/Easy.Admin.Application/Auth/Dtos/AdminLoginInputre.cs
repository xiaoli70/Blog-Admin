﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Admin.Application.Auth.Dtos
{
    public class AdminLoginInputre
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "请输入登录名")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入登录密码")]
        public string Password { get; set; }

        /// <summary>
        /// 验证码唯一ID
        /// </summary>
        [Required(ErrorMessage = "缺少参数")]
        public string Id { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入验证码")]
        public string Code { get; set; }
        //地址
        public string referer { get; set; }
    }
}
