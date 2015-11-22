using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Data;
using DotNet.Data.Repositories;

namespace DotNet.CloudFarm.Domain.DTO.Product
{
    public class PreSaleProductAccess:GenericRepository<PreSaleProduct>,IPreSaleProductAccess
    {
        public PreSaleProductAccess(IEntityFrameworkDbContext context, bool isNoTracking = true) : base(context, isNoTracking)
        {
        }
    }
}
