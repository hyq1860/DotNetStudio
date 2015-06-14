using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.User
{
    public class UserCaptchaModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 状态：0-未验证；1-已验证
        /// </summary>
        public int Status { get; set; }
    }
}
