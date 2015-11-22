using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Address;

namespace DotNet.CloudFarm.Domain.Impl.Address
{
    public class AddressService:IAddressService
    {
        private IAddressDataAccess addressDataAccess;
        public AddressService(IAddressDataAccess addressDataAccess)
        {
            this.addressDataAccess = addressDataAccess;
        }

        public List<Model.Address> GetAddresses()
        {
            return addressDataAccess.GetList().ToList();
        }

        public List<Model.Address> GetChildrenByParentId(string code)
        {
            var address = addressDataAccess.GetById(code);

            return address.Children.ToList();
        }
    }
}
