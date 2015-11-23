using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.CloudFarm.Domain.Model.User;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    /// <summary>
    /// 预售订单
    /// </summary>
    public class PreSaleOrder
    {
        public PreSaleOrder()
        {
            ExpressDelivery = string.Empty;
        }

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

        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 订单状态 未支付0 已支付1 已发货2
        /// </summary>
        public int Status { get; set; }

        public string StatusDesc
        {
            get
            {
                if (Status == 0)
                {
                    return "未支付";
                }
                else if (Status == 1)
                {
                    return "已支付";
                }
                else if (Status == 2)
                {
                    return "已发货";
                }
                return string.Empty;
            }
        }

        ///// <summary>
        ///// 支付方式 0-微信支付；1-线下支付
        ///// </summary>
        //public int PayType { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string ProvinceId { get; set; }

        /// <summary>
        /// 市区县
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        //public virtual Address Province { get; set; }

        //public virtual Address City { get; set; }

        public virtual Address Area { get; set; }

        //public virtual UserModel User { get; set; }

        ///// <summary>
        ///// 订单的贺卡
        ///// </summary>
        //public string Greeting { get; set; }

        /// <summary>
        /// 快递编号
        /// </summary>
        public string ExpressDelivery { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 收货人联系方式
        /// </summary>
        public string Phone { get; set; }

        public DateTime? ModifyTime { get; set; }

        public virtual PreSaleProduct PreSaleProduct { get; set; }
    }

    public class PreSaleOrderEfMap : EntityTypeConfiguration<PreSaleOrder>
    {
        public PreSaleOrderEfMap()
        {
            ToTable("PreSaleOrders");

            HasKey(c => c.OrderId);
            Property(c => c.OrderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(c => c.Code).HasColumnName("AreaId");
            Ignore(c => c.StatusDesc);
        }
    }
}
