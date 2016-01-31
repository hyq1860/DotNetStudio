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
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public int ProductId { get; set; }


        /// <summary>
        /// 购买数量
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 购买单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 支付方式 0-微信支付；1-线下支付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 赠送留言
        /// </summary>
        public string SendRemark { get; set; }

        /// <summary>
        /// 赠送时间
        /// </summary>
        public DateTime? SendDate { get; set; } 

        /// <summary>
        /// 赠送人id 谁赠送的
        /// </summary>
        public int SendUserId { get; set; }
    }
}
