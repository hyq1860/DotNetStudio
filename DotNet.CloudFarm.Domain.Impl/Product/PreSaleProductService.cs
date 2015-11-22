using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;

namespace DotNet.CloudFarm.Domain.Impl.Product
{
    public class PreSaleProductService:IPreSaleProductService
    {
        private IPreSaleProductAccess preSaleProductAccess;
        public PreSaleProductService(IPreSaleProductAccess preSaleProductAccess)
        {
            this.preSaleProductAccess = preSaleProductAccess;
        }

        public List<PreSaleProduct> GetPreSaleProducts()
        {
            return preSaleProductAccess.GetList().ToList();
        }

        public List<PreSaleProduct> GetPreSaleProducts(Expression<Func<PreSaleProduct, bool>> whereFunc, Expression<Func<PreSaleProduct, long>> orderByFunc, string orderType)
        {
            var queryable = preSaleProductAccess.GetList();
            if (orderByFunc != null)
            {
                if (string.IsNullOrEmpty(orderType) || orderType.ToLower() == "desc")
                {
                    queryable = queryable.OrderByDescending(orderByFunc);
                }
                else
                {
                    queryable = queryable.OrderBy(orderByFunc);
                }
            }
            queryable=queryable.Where(whereFunc);
            return queryable.ToList();
        }

        public PreSaleProduct GetPreSaleProduct(int id)
        {
            return preSaleProductAccess.GetById(id);
        }
    }
}
