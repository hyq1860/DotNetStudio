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
    /// 订单数据访问接口
    /// </summary>
    public interface IOrderDataAccess
    {
        /// <summary>
        /// 获取订单交易额排名
        /// </summary>
        /// <param name="top"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<TopOrderInfo> GetTopOrderList(int top, int pageIndex, int pageSize);

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        OrderModel SubmitOrder(OrderModel orderModel);

        /// <summary>
        /// 获取单个订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        OrderModel GetOrder(long orderId,int userId);

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<OrderViewModel> GetOrderList(int userId, int pageIndex, int pageSize);

        Int64 GetNewOrderId();

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        Result<OrderViewModel> UpdateOrderStatus(int userId, long orderId, int orderStatus);


        /// <summary>
        /// 后台订单列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<OrderManageViewModel> GetOrderList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取用户订单在某些状态下的羊的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetProductCountWithStatus(int userId, List<int> status);
    }
}
