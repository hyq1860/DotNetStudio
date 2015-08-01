using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.WeiXin
{
    /// <summary>
    /// 微信支付日志
    /// </summary>
    public class WeixinPayLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string WxOpenId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 0-未成功；1-成功
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
