using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Contract.Order
{
    public interface IPreSaleOrderService
    {
        PagedList<PreSaleOrder> GetPreSaleOrderCollection(Expression<Func<PreSaleOrder, bool>> whereFunc, Expression<Func<PreSaleOrder, long>> orderByFunc, string orderType, int pageIndex, int pageSize);

        Result<PagedList<PreSaleOrder>> GetPreSaleOrderList(Expression<Func<PreSaleOrder, bool>> whereFunc,int pageIndex, int pageSize);

        int SubmitPreSaleOrder(PreSaleOrder preSaleOrder);

        /// <summary>
        /// 获取订单列表（BY USERID）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Result<PagedList<PreSaleOrder>> GetPreSaleOrderList(int userId, int pageIndex, int pageSize);
    }
}
