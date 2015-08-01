using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    /// <summary>
    /// 首页默认viewmodel
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// 我的养只数量
        /// </summary>
        public int SheepCount { get; set; }

        /// <summary>
        /// 首页商品集合
        /// </summary>
        public List<ProductModel> Products { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLogin { get; set; }
    }
}
