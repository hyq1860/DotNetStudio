using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Data;
using DotNet.Data.Repositories;

namespace DotNet.CloudFarm.Domain.DTO.Order
{
    public class PreSaleOrderDataAccess:GenericRepository<PreSaleOrder>,IPreSaleOrderDataAccess
    {
        public PreSaleOrderDataAccess(IEntityFrameworkDbContext context, bool isNoTracking = true) : base(context, isNoTracking)
        {
        }
    }
}
