using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Collections;

namespace DotNet.CloudFarm.Domain.Contract.Product
{
    /// <summary>
    /// yangke每期产品接口
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 获取产品列表(未删除)
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<ProductModel> GetProducts(int pageIndex,int pageSize);

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status">产品状态</param>
        /// <returns></returns>
        PagedList<ProductModel> GetProducts(int pageIndex, int pageSize,int status);

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, string condition);

        /// <summary>
        /// 根据商品id获取商品详情
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductModel GetProductById(int productId);
        /// <summary>
        /// 插入商品
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        int InsertProduct(ProductModel product);

        void UpdateProduct(ProductModel product);
        /// <summary>
        /// 更新product的状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdateStatus(int id,int status);


        /// <summary>
        /// 更新虚拟库存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="VirtualSaledCount"></param>
        void ProductVirtualSaledCount(int id, int VirtualSaledCount);

        /// <summary>
        /// 根据条件获取单个产品
        /// </summary>
        /// <param name="condition"></param>
        ProductModel GetProductByCondition(string condition);
    }
}
