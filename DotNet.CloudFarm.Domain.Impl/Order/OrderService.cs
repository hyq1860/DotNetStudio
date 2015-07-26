
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
using DotNet.CloudFarm.Domain.Model.Base;
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
                Data = orderDataAccess.GetOrderList(userId, pageIndex, pageSize),
                Status = new Status() { Code = "1",Message = ""}
            };
            return result;
        }

        //获取用户的所有有效订单数据
        public Result<List<OrderViewModel>> GetUserAllOrder(int userId,List<int> orderStatus)
        {
            var result = new Result<List<OrderViewModel>>();
            result.Data= orderDataAccess.GetUserAllOrder(userId, orderStatus);
            result.Status = new Status() {Code = "1"};
            return result;
        }

        /// <summary>
        /// 获取用户钱包信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public WalletViewModel GetWalletViewModel(int userId, List<int> orderStatus)
        {
            var walletViewModel = new WalletViewModel();
            var orderList = GetUserAllOrder(userId, orderStatus);
            if (orderList != null && orderList.Data != null && orderList.Data.Any())
            {
                //当前收益
                var currentOrderList =
                    orderList.Data.Where(
                        s =>
                            s.Status == OrderStatus.Paid.GetHashCode() &&
                            s.Status == OrderStatus.WaitingConfirm.GetHashCode());
                var now = DateTime.Now;
                if (currentOrderList != null && currentOrderList.Any())
                {
                    //当前收益
                    foreach (var model in currentOrderList)
                    {
                        decimal rate = 0;
                        if (model.EndTime >= now)
                        {
                            rate = 0;
                        }
                        else if(model.EndTime.AddDays(model.EarningDay)<=now)
                        {
                            rate = 1;
                        }
                        else
                        {
                             rate = (model.EndTime-now).Days/model.EarningDay;
                        }
                        walletViewModel.CurrentIncome+=model.Earning*model.ProductCount*rate;
                    }


                    //预期收益
                    walletViewModel.ExpectIncome = currentOrderList.Sum(s => s.Earning * s.ProductCount);

                    //预期年化收益率
                    walletViewModel.YearEarningRate = currentOrderList.Sum(s => s.YearEarningRate) / currentOrderList.Count();

                    //育肥状态
                    var orderViewModel =
                        currentOrderList.Where(s => s.StartTime < now && s.EndTime <= now)
                            .OrderByDescending(s => s.CreateTime)
                            .FirstOrDefault();

                    if (orderViewModel != null)
                    {
                        walletViewModel.GrowDay = Math.Floor((((decimal)(orderViewModel.EndTime - now).Days) / (decimal)orderViewModel.EarningDay))*100;
                    }
                }
                
                //历史
                var historyOrderList = orderList.Data.Where(s => s.Status == OrderStatus.Complete.GetHashCode());
                if (historyOrderList.Any())
                {
                    //累计收益
                    walletViewModel.TotalIncome = historyOrderList.Sum(s => s.Earning * s.ProductCount);

                    //累计养殖数量
                    walletViewModel.TotalProductCount = historyOrderList.Sum(s => s.ProductCount);

                    //累计投入金额
                    walletViewModel.TotalInvestment = historyOrderList.Sum(s => s.ProductCount * s.Price);
                }
            }

            return walletViewModel;
        }

        public OrderStatisModel GetOrderStatisModel()
        {
            return orderDataAccess.GetOrderStatisModel();
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
            var productModel = productService.GetProductById(orderModel.ProductId);
            if (productModel.CanSale)
            {
                //订单表 扣去库存
                var tempOrderModel = orderDataAccess.SubmitOrder(orderModel);
                if (tempOrderModel.OrderId > 0)
                {

                    result.Data = tempOrderModel;
                    result.Status = new Status() { Code = "1", Message = "提交订单成功。" };
                    UserModel user = userService.GetUserByUserId(tempOrderModel.UserId);
                    //发送消息
                    messageService.SendSms(user.UserId, string.Format("【科羊云牧-羊客】您的订单<a href=\"/Home/OrderList?orderid={0}&tab=3#Order_{0}\">{0}</a>已经提交成功。", tempOrderModel.OrderId));

                    //发送下单成功短信息
                    smsService.SendSMSOrderCreated(user.Mobile, tempOrderModel.OrderId, tempOrderModel.Price * tempOrderModel.ProductCount);
                }
                else
                {
                    result.Status = new Status() { Code = "0", Message = "提交订单失败,请稍后重试。" };
                }
            }
            else
            {
                result.Status = new Status() { Code = "0", Message = "该产品暂无法销售。" };
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

        public bool InsertOrderPay(OrderPayModel orderPayModel)
        {
            return orderDataAccess.InsertOrderPay(orderPayModel);
        }

        public bool UpdateOrderPay(OrderPayModel orderPayModel)
        {
            return orderDataAccess.UpdateOrderPay(orderPayModel);
        }




        public Result<PagedList<OrderManageViewModel>> GetOrderList(int pageIndex, int pageSize, DateTime? startTime, DateTime? endTime, long? orderId, string mobile,int? status)
        {
            var result = new Result<PagedList<OrderManageViewModel>>
            {
                Data = orderDataAccess.GetOrderList(pageIndex, pageSize,startTime,endTime,orderId,mobile,status)
            };
            return result;
        }
    }
}
