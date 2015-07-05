using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Contract.Order
{
    /// <summary>
    /// 订单数据访问接口
    /// </summary>
    public interface IOrderDataAccess
    {
        List<TopOrderInfo> GetTopOrderList(int top, int pageIndex, int pageSize);

        Result<OrderModel> SubmitOrder(OrderModel orderModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Result<OrderModel> GetOrder(long orderId,int userId);
    }
}
