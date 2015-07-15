using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.ViewModel;
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
        /// 获取订单列表（BY USERID）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Result<PagedList<OrderViewModel>> GetOrderList(int userId, int pageIndex, int pageSize);

        /// <summary>
        /// 获取订单列表（ALL）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Result<PagedList<OrderManageViewModel>> GetOrderList(int pageIndex, int pageSize);

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        Result<OrderModel> SubmitOrder(OrderModel orderModel);

       /// <summary>
        /// 获取订单销量排行榜
       /// </summary>
       /// <param name="top"></param>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
        List<TopOrderInfo> GetTopOrderList(int top, int pageIndex, int pageSize);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderModel GetOrder(int userId, long orderId);

        Int64 GetNewOrderId();

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        Result<OrderViewModel> UpdateOrderStatus(int userId, long orderId,int orderStatus);
    }
}
