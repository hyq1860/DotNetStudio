using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Base;
using DotNet.CloudFarm.Domain.Model.Order;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    public class OrderViewModel:OrderModel
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片url
        /// </summary>
        public string ProductImgUrl { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusDesc 
        {
            get
            {
                var orderStatusDesc = string.Empty;
                switch (Status)
                {
                    case -1:
                        orderStatusDesc = "交易关闭";
                        break;
                    case 0:
                        orderStatusDesc = "待支付";
                        break;
                    case 1:
                        orderStatusDesc = "已支付";
                        break;
                    case 2:
                        orderStatusDesc = "待确认赎回";
                        break;
                    case 10:
                        orderStatusDesc = "完成";
                        break;
                }
                return orderStatusDesc;
            } 
        }

        /// <summary>
        /// 预期年化收益率
        /// </summary>
        public decimal YearEarningRate { get; set; }

        /// <summary>
        /// 收益率
        /// </summary>
        public decimal EarningRate { get; set; }

        /// <summary>
        /// 预期收益
        /// </summary>
        public decimal Earning { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动持续时间、天（预计收益时间）
        /// </summary>
        public int EarningDay { get; set; }

        /// <summary>
        /// 是否可以赎回
        /// </summary>
        public bool CanRedeem {
            get { return EndTime.AddDays(EarningDay) <= DateTime.Now && Status == OrderStatus.Paid.GetHashCode(); } }

        /// <summary>
        /// 是否可以支付
        /// </summary>
        public bool CanPay
        {
            get { return Status == OrderStatus.Init.GetHashCode() && DateTime.Now<EndTime; }
        }

        /// <summary>
        /// 是否可以赠送
        /// </summary>
        public bool CanSend
        {
            get { return (Status == OrderStatus.Paid.GetHashCode()) && SendUserId <= 0; }
        }

        /// <summary>
        /// 是否是红包订单
        /// </summary>
        public bool IsSendOrder
        {
            get { return SendUserId > 0; }
        }

        /// <summary>
        /// 赠送者昵称
        /// </summary>
        public string SendUserName { get; set; }

        /// <summary>
        /// 赠送这手机号
        /// </summary>
        public string SendUserMobile { get; set; }
    }
}