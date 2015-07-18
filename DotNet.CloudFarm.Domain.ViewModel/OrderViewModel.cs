using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
