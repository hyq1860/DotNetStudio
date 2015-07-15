using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.SMS
{
    /// <summary>
    /// 发短信请求接口实体类
    /// </summary>
    public class SMSRequestModel
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 接收方号码,不支持0打头的号码
        /// </summary>
        public string acceptor_tel { get; set; }

        /// <summary>
        /// 短信模板ID，到短信模板申请页面查看
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 模板匹配参数,参数格式为(json对象字符串): {参数名：参数值，参数名：参数值}
        /// </summary>
        public string template_param { get; set; }

        /// <summary>
        /// 时间戳，格式为：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string timestamp { get; set; }

        /// <summary>
        /// 参数签名
        /// </summary>
        public string sign { get; set; }

    }
}
