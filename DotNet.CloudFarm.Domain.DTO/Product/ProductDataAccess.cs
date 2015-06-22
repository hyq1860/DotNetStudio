using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Collections;

namespace DotNet.CloudFarm.Domain.DTO.Product
{
    /// <summary>
    /// 产品数据访问层
    /// </summary>
    public class ProductDataAccess:IProductDataAccess
    {
        public PagedList<ProductModel> GetProducts(int pageIndex, int pageSize, int status)
        {
            var data = new List<ProductModel>();
            var result = new PagedList<ProductModel>(data,pageIndex,pageSize);
            result.TotalCount = 100;//总记录数量
            result.PageCount = 10;//总页数
            return result;
        }
    }
}
