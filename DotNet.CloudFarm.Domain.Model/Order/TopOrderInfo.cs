using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.User;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    /// <summary>
    /// 订单排名实体
    /// </summary>
    public class TopOrderInfo
    {
        public int UserId { get; set; }

        public string Mobile { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WxNickName { get; set; }

        public string HeadUrl { get; set; }

        public decimal Total { get; set; }
    }
}
