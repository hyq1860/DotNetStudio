
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Message;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Contract.SMS;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl.Order
{
    public class OrderService:IOrderService
    {

        private IOrderDataAccess orderDataAccess;

        private ISMSService smsService;

        private IUserService userService;

        private IProductService productService;

        private IMessageService messageService;

        public OrderService(IOrderDataAccess orderDataAccess, ISMSService smsService, IUserService userService, IProductService productService, IMessageService messageService)
        {
            this.orderDataAccess = orderDataAccess;
            this.smsService = smsService;
            this.userService = userService;
            this.productService = productService;
            this.messageService = messageService;
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

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public Result<OrderModel> SubmitOrder(OrderModel orderModel)
        {
            var result = new Result<OrderModel>();
            //订单表 扣去库存
            var tempOrderModel = orderDataAccess.SubmitOrder(orderModel);
            if (tempOrderModel.OrderId > 0)
            {
                
                result.Data = tempOrderModel;
                result.Status=new Status(){Code="1",Message = "提交订单成功。"};
                UserModel user= userService.GetUserByUserId(tempOrderModel.UserId);
                //发送消息
                messageService.SendSms(user.UserId, string.Format("【科羊云牧-羊客】您的订单<a href=\"/Home/OrderList?orderid={0}#Order_{0}\">{0}</a>已经提交成功。", tempOrderModel.OrderId));

                //发送下单成功短信息
                smsService.SendSMSOrderCreated(user.Mobile, tempOrderModel.OrderId,tempOrderModel.Price*tempOrderModel.ProductCount);
            }
            else
            {
                result.Status = new Status() { Code = "0", Message = "提交订单失败,请稍后重试。" };
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


        public OrderViewModel GetOrderViewModel(int userId, long orderId)
        {
            return orderDataAccess.GetOrderViewModel(orderId, userId);
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
