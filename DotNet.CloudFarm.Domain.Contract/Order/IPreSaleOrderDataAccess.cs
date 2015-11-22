using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.Contract.Order
{
    public interface IPreSaleOrderDataAccess:IRepository<PreSaleOrder>
    {
        
    }
}
