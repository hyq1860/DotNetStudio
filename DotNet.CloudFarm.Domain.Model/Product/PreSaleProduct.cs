using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Product
{
    /// <summary>
    /// 预售商品model
    /// </summary>
    public class PreSaleProduct
    {
        public PreSaleProduct()
        {
            CreateTime=new DateTime();
        }
        /// <summary>
        /// 预售商品id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 预售商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预售商品imageurl
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 产品单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 市场参考价
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 保质期
        /// </summary>
        public string ShelfLife { get; set; }

        /// <summary>
        /// 储存条件
        /// </summary>
        public string StorageCondition { get; set; }

        /// <summary>
        /// 配送说明
        /// </summary>
        public string DeliveryArea { get; set; }

        /// <summary>
        /// 礼盒明细描述 html
        /// </summary>
        public string PackageDetail { get; set; }

        /// <summary>
        /// 礼盒内内容
        /// </summary>
        public string PackageIn { get; set; }

        /// <summary>
        /// 产品规格描述
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 产品包装描述
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<PreSaleProductDetail> Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DetailJson { get; set; }

        /// <summary>
        /// 是否销售
        /// </summary>
        public bool IsSale { get; set; }
    }

    public class PreSaleProductDetail
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Weight { get; set; }

        public string Count { get; set; }
    }

    public class PreSaleProductEfMap:EntityTypeConfiguration<PreSaleProduct>
    {
        public PreSaleProductEfMap()
        {
            ToTable("PreSaleProducts");
            HasKey(c => c.ProductId);
            Property(c => c.ProductId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Ignore(c => c.Details);
        }
    }
}
