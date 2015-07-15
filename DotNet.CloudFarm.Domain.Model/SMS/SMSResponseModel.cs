using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.SMS
{
    /// <summary>
    /// 调取短信接口返回实体类
    /// </summary>
    public class SMSResponseModel
    {
        /// <summary>
        /// 标准返回码。返回0表示成功；其他表示调用出错或异常
        /// </summary>
        public int res_code { get; set; }

        /// <summary>
        /// 返回码描述信息
        /// </summary>
        public string res_message { get; set; }

        /// <summary>
        /// 与请求参数中state的值一致
        /// </summary>
        public string state { get; set; }

        /// <summary>
        /// 获取到的访问令牌（AT或UIAT）
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 访问令牌的有效期（以秒为单位）
        /// </summary>
        public long expires_in { get; set; }

        /// <summary>
        /// 获取到的刷新令牌，与获取到的访问令牌相对应
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 权限列表，保留字段，默认为空
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 成功返回：短信唯一标识；错误返回：返回空
        /// </summary>
        public string identifier { get; set; }
    }
}
