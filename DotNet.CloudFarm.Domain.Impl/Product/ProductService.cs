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

        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize)
        {
            return productDataAccess.GetProducts(pageIndex, pageSize);
        }

        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, int status)
        {
            return productDataAccess.GetProducts(pageIndex, pageSize, status);

        }

        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, string condition)
        {
            return productDataAccess.GetProducts(pageIndex, pageSize, condition);
        }

        public ProductModel GetProductById(int productId)
        {
            return productDataAccess.GetProductById(productId);
        }


        public int InsertProduct(ProductModel product)
        {
            return productDataAccess.InserProduct(product);
        }


        public void UpdateProduct(ProductModel product)
        {
             productDataAccess.UpdateProduct(product);
        }




        public void UpdateStatus(int id, int status)
        {
            productDataAccess.UpdateStatus(id,status);
        }
    }
}
