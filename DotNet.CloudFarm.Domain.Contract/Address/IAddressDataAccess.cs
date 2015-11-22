using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.Contract.Address
{
    public interface IAddressDataAccess:IRepository<Model.Address>
    {
    }
}
