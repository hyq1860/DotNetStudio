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
        /// 获取订单列表（条件）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="orderId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Result<PagedList<OrderManageViewModel>> GetOrderList(int pageIndex, int pageSize,DateTime? startTime,DateTime? endTime,long? orderId,string mobile,int? status);

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        Result<OrderModel> SubmitOrder(OrderModel orderModel);

       /// <summary>
        /// 获取订单销量排行榜
       /// </summary>
       /// <param name="pageIndex"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
        List<TopOrderInfo> GetTopOrderList(int pageIndex, int pageSize);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderModel GetOrder(int userId, long orderId);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderViewModel GetOrderViewModel(int userId, long orderId);

        Int64 GetNewOrderId();

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        Result<OrderViewModel> UpdateOrderStatus(int userId, long orderId,int orderStatus);

        /// <summary>
        /// 获取用户订单在某些状态下的羊的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetProductCountWithStatus(int userId,List<int> status);

        /// <summary>
        /// 创建支付记录
        /// </summary>
        /// <param name="orderPayModel"></param>
        /// <returns></returns>
        bool InsertOrderPay(OrderPayModel orderPayModel);

        /// <summary>
        /// 更新支付状态
        /// </summary>
        /// <param name="orderPayModel"></param>
        /// <returns></returns>
        bool UpdateOrderPay(OrderPayModel orderPayModel);

        //获取用户的所有有效订单数据
        Result<List<OrderViewModel>> GetUserAllOrder(int userId, List<int> orderStatus);

        /// <summary>
        /// 获取用户钱包实体
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        WalletViewModel GetWalletViewModel(int userId,List<int> orderStatus);

        /// <summary>
        /// 订单统计信息
        /// </summary>
        /// <returns></returns>
        OrderStatisModel GetOrderStatisModel(List<int> status);

        /// <summary>
        /// 修改订单支付方式
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="payType"></param>
        /// <returns></returns>
        bool UpdateOrderPayType(long orderId, int userId, int payType);

        /// <summary>
        /// 根据用户id和分页信息获取订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Result<PagedList<PreSaleOrder>> GetPreSaleOrderList(int userId, int pageIndex, int pageSize);
    }
}
