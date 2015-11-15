using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.Model
{
    public class CloudFarmDbContext:EntityFrameworkDbContext
    {
        public CloudFarmDbContext():base("YangkeConnection")
        {
        }
        public CloudFarmDbContext(string connectionString) : base(connectionString)
        {
        }

        public CloudFarmDbContext(string connectionString, bool autoDetectChangesEnabled) : base(connectionString, autoDetectChangesEnabled)
        {
        }

        /// <summary>
        /// 预售商品集合
        /// </summary>
        public DbSet<PreSaleProduct> PreSaleProducts { get; set; }

        /// <summary>
        /// 预售订单集合
        /// </summary>
        public DbSet<PreSaleOrder> PreSaleOrders { get; set; }
    }
}
