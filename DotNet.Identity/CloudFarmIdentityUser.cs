using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DotNet.Identity
{
    /// <summary>
    /// yangke的用户认证对象
    /// </summary>
    public class CloudFarmIdentityUser : IUser
    {
        /// <summary>
        /// 用户UserId
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string MobileCaptcha { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public int Role { get; set; }
    }
}
