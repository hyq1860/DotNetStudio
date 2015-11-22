using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.Contract.Product
{
    public interface IPreSaleProductAccess:IRepository<PreSaleProduct>
    {
    }
}
