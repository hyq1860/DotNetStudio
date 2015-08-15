using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    /// <summary>
    /// 支付提醒viewmodel
    /// </summary>
    public class PayTipViewModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 购买的养只数量
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 是否支付成功
        /// </summary>
        public bool IsPaySuccess { get; set; }

        /// <summary>
        /// 支付提示语
        /// </summary>
        public string Message { get; set; }
    }
}
