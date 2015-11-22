using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Address;
using DotNet.Data;
using DotNet.Data.Repositories;

namespace DotNet.CloudFarm.Domain.DTO.Address
{
    public class AddressDataAccess: GenericRepository<Model.Address>,IAddressDataAccess
    {
        public AddressDataAccess(IEntityFrameworkDbContext context, bool isNoTracking = true) : base(context, isNoTracking)
        {
        }
    }
}
