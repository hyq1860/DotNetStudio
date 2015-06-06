using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    /// <summary>
    /// 订单支付流水实体
    /// </summary>
    public class OrderPayModel
    {
        /// <summary>
        /// 支付流水编号
        /// </summary>
        public long PayId { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrdeId { get; set; }
    }
}
