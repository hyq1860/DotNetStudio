using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Product;

namespace DotNet.CloudFarm.Domain.Contract.Product
{
    public interface IPreSaleProductService
    {
        /// <summary>
        /// 获取预售商品
        /// </summary>
        /// <returns></returns>
        List<PreSaleProduct> GetPreSaleProducts();

        List<PreSaleProduct> GetPreSaleProducts(Expression<Func<PreSaleProduct, bool>> whereFunc,
            Expression<Func<PreSaleProduct, long>> orderByFunc, string orderType);

        /// <summary>
        /// 根据商品id获取预售商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PreSaleProduct GetPreSaleProduct(int id);
    }
}
