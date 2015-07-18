
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl.Order
{
    public class OrderService:IOrderService
    {

        private IOrderDataAccess orderDataAccess;

        public OrderService(IOrderDataAccess orderDataAccess)
        {
            this.orderDataAccess = orderDataAccess;
        }

        public Result<PagedList<OrderViewModel>> GetOrderList(int userId, int pageIndex, int pageSize)
        {
            var result = new Result<PagedList<OrderViewModel>>
            {
                Data = orderDataAccess.GetOrderList(userId, pageIndex, pageSize)
            };
            return result;
        }

        public Result<PagedList<OrderManageViewModel>> GetOrderList(int pageIndex, int pageSize)
        {
            var result = new Result<PagedList<OrderManageViewModel>>
            {
                Data = orderDataAccess.GetOrderList(pageIndex, pageSize)
            };
            return result;
        }

        public Result<OrderModel> SubmitOrder(OrderModel orderModel)
        {
            //提交订单
            var result = new Result<OrderModel>();
            var tempOrderModel = orderDataAccess.SubmitOrder(orderModel);
            if (tempOrderModel.OrderId > 0)
            {
                result.Data = tempOrderModel;
                result.Status=new Status(){Code="1",Message = "提交订单成功。"};
            }

            return result;
        }

        public List<TopOrderInfo> GetTopOrderList(int pageIndex, int pageSize)
        {
            return orderDataAccess.GetTopOrderList(pageIndex, pageSize);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public OrderModel GetOrder(int userId, long orderId)
        {
            return orderDataAccess.GetOrder(orderId, userId);
        }

        public long GetNewOrderId()
        {
            return orderDataAccess.GetNewOrderId();
        }

        public Result<OrderViewModel> UpdateOrderStatus(int userId, long orderId, int orderStatus)
        {
            return orderDataAccess.UpdateOrderStatus(userId, orderId, orderStatus);
        }

        public int GetProductCountWithStatus(int userId, List<int> status)
        {
            return orderDataAccess.GetProductCountWithStatus(userId, status);
        }
    }
}
