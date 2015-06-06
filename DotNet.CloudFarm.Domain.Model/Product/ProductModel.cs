using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime End { get; set; }
    }
}
