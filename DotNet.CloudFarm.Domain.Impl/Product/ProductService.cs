using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Collections;

namespace DotNet.CloudFarm.Domain.Impl.Product
{
    /// <summary>
    /// 产品service
    /// </summary>
    public class ProductService:IProductService
    {
        private IProductDataAccess productDataAccess;
        public ProductService(IProductDataAccess productDataAccess)
        {
            this.productDataAccess = productDataAccess;
        }

        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, int status)
        {
            return productDataAccess.GetProducts(pageIndex, pageSize, status);
        }
    }
}
