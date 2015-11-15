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
        /// <summary>
        /// 预售商品id
        /// </summary>
        public int Id { get; set; }

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
    }

    public class PreSaleProductEfMap:EntityTypeConfiguration<PreSaleProduct>
    {
        public PreSaleProductEfMap()
        {
            ToTable("PreSaleProducts");
            HasKey(c => c.Id);
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
