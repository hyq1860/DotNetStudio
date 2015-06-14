using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Contract.Order
{
    /// <summary>
    /// 订单接口
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 获取订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<OrderModel> GetOrderList(int userId,int pageIndex,int pageSize);

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        Result<OrderModel> SubmitOrder(OrderModel orderModel);
    }
}
