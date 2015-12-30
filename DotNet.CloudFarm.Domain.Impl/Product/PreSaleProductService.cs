using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.Common.Utility;

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
            var dataList = queryable.ToList();
            foreach (var product in dataList)
            {
                product.Details = JsonHelper.FromJson<List<PreSaleProductDetail>>(product.DetailJson);
            }
            return dataList;
        }

        public List<PreSaleProduct> GetPreSaleProducts(Expression<Func<PreSaleProduct, bool>> whereFunc, Expression<Func<PreSaleProduct, DateTime>> orderByFunc, string orderType)
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
            queryable = queryable.Where(whereFunc);
            var dataList = queryable.ToList();
            foreach (var product in dataList)
            {
                product.Details = JsonHelper.FromJson<List<PreSaleProductDetail>>(product.DetailJson);
            }
            return dataList;
        }

        public PreSaleProduct GetPreSaleProduct(int id)
        {
            return preSaleProductAccess.GetById(id);
        }

        public bool Add(PreSaleProduct preSaleProduct)
        {
            return preSaleProductAccess.Add(preSaleProduct)>0;
        }

        public bool Update(PreSaleProduct preSaleProduct)
        {
            return preSaleProductAccess.Update(preSaleProduct)>0;
        }
    }
}
