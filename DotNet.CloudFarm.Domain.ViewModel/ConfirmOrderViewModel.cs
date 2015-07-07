using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.Product;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    /// <summary>
    /// 确认订单viewmodel
    /// </summary>
    public class ConfirmOrderViewModel
    {
        /// <summary>
        /// 商品
        /// </summary>
        public ProductModel Product { get; set; }

        //购买数量
        public int Count { get; set; }

        /// <summary>
        /// 用户top信息
        /// </summary>
        public List<TopOrderInfo> TopOrderInfos { get; set; } 
    }
}
