using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime InDate { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime EditDate { get; set; }

        /// <summary>
        /// 订单详情集合
        /// </summary>
        public List<OrderDetailModel> OrderDetails { get; set; } 
    }
}
