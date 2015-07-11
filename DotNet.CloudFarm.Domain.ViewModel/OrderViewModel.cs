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
    }
}
