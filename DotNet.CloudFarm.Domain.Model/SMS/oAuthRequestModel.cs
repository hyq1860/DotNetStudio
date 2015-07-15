using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.SMS
{
    public class oAuthRequestModel
    {
        /// <summary>
        /// 授权模式
        /// </summary>
        public EnumGrantType grant_type { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        public string app_secret { get; set; }
        /// <summary>
        /// 用于跟踪调用状态。在响应消息中将会原封不动的返回该值
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 权限列表，保留字段，默认为空
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 用于刷新访问令牌所需的更新令牌
        /// </summary>
        public string refresh_token { get; set; }
     

    }

    public enum EnumGrantType
    {
        /// <summary>
        /// AC授权
        /// </summary>
        authorization_code,
        /// <summary>
        /// CC授权
        /// </summary>
        client_credentials,
        /// <summary>
        /// 刷新授权
        /// </summary>
        refresh_token
    }
}
