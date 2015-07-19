using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.User
{
    /// <summary>
    /// 登陆用户
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }

        /// <summary>
        /// 用户的微信OpenId
        /// </summary>
        public string WxOpenId { get; set; }

    }
}
