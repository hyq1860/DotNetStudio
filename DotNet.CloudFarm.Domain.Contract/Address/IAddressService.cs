using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model;
namespace DotNet.CloudFarm.Domain.Contract.Address
{
    public interface IAddressService
    {
        List<Model.Address> GetAddresses();

        List<Model.Address> GetChildrenByParentId(string code);
    }
}
