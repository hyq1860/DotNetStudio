using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.User;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    /// <summary>
    /// 预售订单
    /// </summary>
    public class PreSaleOrder
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 购买单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        ///// <summary>
        ///// 支付方式 0-微信支付；1-线下支付
        ///// </summary>
        //public int PayType { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 市区县
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        public virtual UserModel User { get; set; }

        ///// <summary>
        ///// 订单的贺卡
        ///// </summary>
        //public string Greeting { get; set; }
    }

    public class PreSaleOrderEfMap : EntityTypeConfiguration<PreSaleOrder>
    {
        public PreSaleOrderEfMap()
        {
            ToTable("PreSaleOrders");

            HasKey(c => c.OrderId);
            Property(c => c.OrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
