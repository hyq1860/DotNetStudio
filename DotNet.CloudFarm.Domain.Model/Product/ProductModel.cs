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
        /// 产品id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品总数
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 当前期销售数量
        /// </summary>
        public int SaledCount { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 羊金额
        /// </summary>
        public decimal SheepPrice { get; set; }

        /// <summary>
        /// 饲料金额
        /// </summary>
        public decimal FeedPrice { get; set; }

        /// <summary>
        /// 人工成本
        /// </summary>
        public decimal WorkPrice { get; set; }

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
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否显示  0 不显示 1显示
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建者id
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 活动持续时间、天（预计收益时间）
        /// </summary>
        public int EarningDay { get; set; }

        /// <summary>
        /// 羊羔品种
        /// </summary>
        public string SheepType { get; set; }

        /// <summary>
        /// 育肥场
        /// </summary>
        public string SheepFactory { get; set; }
        
        /// <summary>
        /// 产品图片URL
        /// </summary>
        public string  ImgUrl { get; set; }
    }
}
