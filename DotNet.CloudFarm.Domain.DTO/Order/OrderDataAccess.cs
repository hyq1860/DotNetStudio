using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.Common.Collections;
using DotNet.Common.Models;
using DotNet.Data;
using System.Data;

namespace DotNet.CloudFarm.Domain.DTO.Order
{
    public class OrderDataAccess:IOrderDataAccess
    {
        private IProductDataAccess ProductDataAccess;

        public OrderDataAccess(IProductDataAccess productDataAccess)
        {
            this.ProductDataAccess = productDataAccess;
        }

        public List<TopOrderInfo> GetTopOrderList(int pageIndex, int pageSize)
        {
            var result = new List<TopOrderInfo>();
            using (var cmd = DataCommandManager.GetDataCommand("GetTopOrderList"))
            {
                cmd.SetParameterValue("@PageIndex",pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                using (var dr=cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var topOrderInfo = new TopOrderInfo();
                        topOrderInfo.UserId = !Convert.IsDBNull(dr["UserId"]) ? int.Parse(dr["UserId"].ToString()) : 0;
                        topOrderInfo.Mobile = !Convert.IsDBNull(dr["Mobile"]) ? dr["Mobile"].ToString() : string.Empty;
                        topOrderInfo.BuyCount = !Convert.IsDBNull(dr["Total"]) ? decimal.Parse(dr["Total"].ToString()) : 0;
                        topOrderInfo.HeadUrl = !Convert.IsDBNull(dr["WxHeadUrl"]) ? dr["WxHeadUrl"].ToString() : string.Empty;
                        result.Add(topOrderInfo);
                    }
                }   
            }
            return result;
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        public OrderModel SubmitOrder(OrderModel orderModel)
        {
            var result = new OrderModel();
            using (var cmd = DataCommandManager.GetDataCommand("InsertOrder"))
            {
                cmd.SetParameterValue("@OrderId", orderModel.OrderId);
                cmd.SetParameterValue("@UserId", orderModel.UserId);
                cmd.SetParameterValue("@ProductId", orderModel.ProductId);
                cmd.SetParameterValue("@ProductCount", orderModel.ProductCount);
                cmd.SetParameterValue("@Price", orderModel.Price);
                cmd.SetParameterValue("@Status", orderModel.Status);
                cmd.SetParameterValue("@PayType", orderModel.PayType);
                cmd.SetParameterValue("@CreateTime", orderModel.CreateTime);

                var resultInt = cmd.ExecuteNonQuery();
                if (resultInt > 0)
                {
                    result = GetOrder(orderModel.OrderId, orderModel.UserId);
                }
            }
            return result;
        }

        public OrderModel GetOrder(long orderId, int userId)
        {
            var orderModel = new OrderModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetOrder"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@OrderId", orderId);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        orderModel.OrderId = !Convert.IsDBNull("OrderId") ? long.Parse(dr["OrderId"].ToString()) : 0;
                        orderModel.UserId = !Convert.IsDBNull("UserId") ? int.Parse(dr["UserId"].ToString()) : 0;
                        orderModel.ProductId = !Convert.IsDBNull("ProductId") ? int.Parse(dr["ProductId"].ToString()) : 0;
                        orderModel.ProductCount = !Convert.IsDBNull("ProductCount") ? int.Parse(dr["ProductCount"].ToString()) : 0;
                        orderModel.Price = !Convert.IsDBNull("Price") ? decimal.Parse(dr["Price"].ToString()) : 0;
                        orderModel.Status = !Convert.IsDBNull("Status") ? int.Parse(dr["Status"].ToString()) : 0;
                        orderModel.PayType = !Convert.IsDBNull("PayType") ? int.Parse(dr["PayType"].ToString()) : 0;
                        orderModel.CreateTime = !Convert.IsDBNull("CreateTime") ? DateTime.Parse(dr["CreateTime"].ToString()) : DateTime.MinValue;
                    }
                }
            }

            return orderModel;
        }

        public PagedList<OrderViewModel> GetOrderList(int userId, int pageIndex, int pageSize)
        {
            var orderList = new List<OrderViewModel>();
            using (var cmd = DataCommandManager.GetDataCommand("GetOrderList"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var orderViewModel = new OrderViewModel();

                        orderViewModel.OrderId = !Convert.IsDBNull(dr["OrderId"]) ? Convert.ToInt64(dr["OrderId"]) : 0;
                        orderViewModel.UserId = !Convert.IsDBNull(dr["UserId"]) ? Convert.ToInt32(dr["UserId"]) : 0;
                        orderViewModel.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                        orderViewModel.ProductId = !Convert.IsDBNull(dr["ProductId"]) ? Convert.ToInt32(dr["ProductId"]) : 0;
                        orderViewModel.ProductCount = !Convert.IsDBNull(dr["ProductCount"]) ? Convert.ToInt32(dr["ProductCount"]) : 0;
                        orderViewModel.Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToDecimal(dr["Price"]) : 0;
                        orderViewModel.Status = !Convert.IsDBNull(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                        orderViewModel.PayType = !Convert.IsDBNull(dr["PayType"]) ? Convert.ToInt32(dr["PayType"]) : 0;
                        orderViewModel.ProductName = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
                        orderViewModel.TotalMoney = !Convert.IsDBNull(dr["TotalMoney"]) ? Convert.ToDecimal(dr["TotalMoney"]) : 0;
                        if (orderViewModel.OrderId > 0)
                        {
                            orderList.Add(orderViewModel);
                        }
                    }
                }
            }
            var result = new PagedList<OrderViewModel>(orderList, pageIndex, pageSize);
            return result;
        }


        /// <summary>
        /// 获取新的订单号,订单号规则：81yyMMdd100001序列
        /// </summary>
        /// <returns></returns>
        public Int64 GetNewOrderId()
        {
            using (var cmd = DataCommandManager.GetDataCommand("GetNewOrderId"))
            {
                var orderId = cmd.ExecuteScalar();
                if (orderId != null)
                {
                    return Convert.ToInt64(orderId);
                }
                return 0;
            }
        }

        public Result<OrderViewModel> UpdateOrderStatus(int userId, long orderId, int orderStatus)
        {
            var result = new Result<OrderViewModel>();
            using (var cmd = DataCommandManager.GetDataCommand("UpdateOrderStatus"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@OrderId", orderId);
                cmd.SetParameterValue("@Status", orderStatus);
                var returnValue = cmd.ExecuteNonQuery();

                result.Status = returnValue > 0 ? new Status() {Code = "1"} : new Status() { Code = "0" };
                var orderModel = GetOrder(orderId, userId);
                if (orderModel != null&&orderModel.ProductId>0)
                {
                    var productModel = ProductDataAccess.GetProductById(orderModel.ProductId);
                    result.Data = OrderModelToOrderViewModel(orderModel, productModel);
                }
                   
                return result;
            }
        }

        private OrderViewModel OrderModelToOrderViewModel(OrderModel orderModel,ProductModel productModel)
        {
            var orderViewModel = new OrderViewModel();
            if (orderModel != null)
            {
                orderViewModel.OrderId = orderModel.OrderId;
                orderViewModel.UserId = orderModel.UserId;
                orderViewModel.CreateTime = orderModel.CreateTime;
                orderViewModel.ProductId = orderModel.ProductId;
                orderViewModel.ProductCount = orderModel.ProductCount;
                orderViewModel.Price = orderModel.Price;
                orderViewModel.Status = orderModel.Status;
                orderViewModel.PayType = orderModel.PayType;
                orderViewModel.TotalMoney = orderModel.Price*orderModel.ProductCount;
            }
            if (productModel != null)
            {
                orderViewModel.ProductName = productModel.Name;
            }
            
            return orderViewModel;
        }


        public PagedList<OrderManageViewModel> GetOrderList(int pageIndex, int pageSize)
        {
            var orderList = new List<OrderManageViewModel>();
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetOrderListAll"))
            {
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                using (var ds = cmd.ExecuteDataSet())
                {
                    if (ds.Tables.Count >= 2)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var omvm = new OrderManageViewModel();

                            omvm.OrderId = !Convert.IsDBNull(dr["OrderId"]) ? Convert.ToInt64(dr["OrderId"]) : 0;
                            omvm.UserId = !Convert.IsDBNull(dr["UserId"]) ? Convert.ToInt32(dr["UserId"]) : 0;
                            omvm.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                            omvm.ProductId = !Convert.IsDBNull(dr["ProductId"]) ? Convert.ToInt32(dr["ProductId"]) : 0;
                            omvm.ProductCount = !Convert.IsDBNull(dr["ProductCount"]) ? Convert.ToInt32(dr["ProductCount"]) : 0;
                            omvm.Price = !Convert.IsDBNull(dr["Price"]) ? Convert.ToDecimal(dr["Price"]) : 0;
                            omvm.Status = !Convert.IsDBNull(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                            omvm.PayType = !Convert.IsDBNull(dr["PayType"]) ? Convert.ToInt32(dr["PayType"]) : 0;
                            omvm.ProductName = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
                            omvm.TotalMoney = !Convert.IsDBNull(dr["TotalMoney"]) ? Convert.ToDecimal(dr["TotalMoney"]) : 0;
                            omvm.Mobile = !Convert.IsDBNull(dr["Mobile"]) ? dr["Mobile"].ToString() : string.Empty;
                            if (omvm.OrderId > 0)
                            {
                                orderList.Add(omvm);
                            }
                        }
                        var countDr = ds.Tables[1].Rows[0][0];
                        count = !Convert.IsDBNull(countDr) ? Convert.ToInt32(countDr) : 0;

                    }
                }
            }
            var result = new PagedList<OrderManageViewModel>(orderList, pageIndex, pageSize,count);
            return result;
        }

        public int GetProductCountWithStatus(int userId, List<int> status)
        {
            var count = 0;
            using (var cmd = DataCommandManager.GetDataCommand("GetProductCountWithStatus"))
            {
                cmd.CommandText = string.Format(cmd.CommandText, string.Join(",", status));
                cmd.SetParameterValue("@UserId", userId);

                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        count = !Convert.IsDBNull(dr[0]) ? Convert.ToInt32(dr[0]) : 0;
                    }
                }
            }
            return count;
        }


        public OrderViewModel GetOrderViewModel(long orderId, int userId)
        {
            var orderModel = new OrderViewModel();
            using (var cmd = DataCommandManager.GetDataCommand("GetOrderViewModel"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@OrderId", orderId);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        orderModel.OrderId = !Convert.IsDBNull("OrderId") ? long.Parse(dr["OrderId"].ToString()) : 0;
                        orderModel.UserId = !Convert.IsDBNull("UserId") ? int.Parse(dr["UserId"].ToString()) : 0;
                        orderModel.ProductId = !Convert.IsDBNull("ProductId") ? int.Parse(dr["ProductId"].ToString()) : 0;
                        orderModel.ProductCount = !Convert.IsDBNull("ProductCount") ? int.Parse(dr["ProductCount"].ToString()) : 0;
                        orderModel.Price = !Convert.IsDBNull("Price") ? decimal.Parse(dr["Price"].ToString()) : 0;
                        orderModel.Status = !Convert.IsDBNull("Status") ? int.Parse(dr["Status"].ToString()) : 0;
                        orderModel.PayType = !Convert.IsDBNull("PayType") ? int.Parse(dr["PayType"].ToString()) : 0;
                        orderModel.ProductName = !Convert.IsDBNull(dr["ProductName"]) ? dr["ProductName"].ToString() : string.Empty;
                        orderModel.TotalMoney = !Convert.IsDBNull(dr["TotalMoney"]) ? Convert.ToDecimal(dr["TotalMoney"]) : 0;
                        orderModel.CreateTime = !Convert.IsDBNull("CreateTime") ? DateTime.Parse(dr["CreateTime"].ToString()) : DateTime.MinValue;
                    }
                }
            }

            return orderModel;
        }

        public List<OrderViewModel> GetUserAllOrder(int userId, List<int> orderStatus)
        {
            var orderList = new List<OrderViewModel>();

            return orderList;
        }

        public bool InsertOrderPay(OrderPayModel orderPayModel)
        {
            using (var cmd = DataCommandManager.GetDataCommand("InsertOrderPay"))
            {
                cmd.SetParameterValue("@PayId", orderPayModel.PayId);
                cmd.SetParameterValue("@OrderId", orderPayModel.OrdeId);
                cmd.SetParameterValue("@UserId", orderPayModel.UserId);
                cmd.SetParameterValue("@Status", orderPayModel.Status);
                cmd.SetParameterValue("@CreateTime", orderPayModel.CreateTime);

                return cmd.ExecuteNonQuery()>0;
            }
        }

        public bool UpdateOrderPay(OrderPayModel orderPayModel)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateOrderPay"))
            {
                //cmd.SetParameterValue("@OrdeId", orderPayModel.OrdeId);
                cmd.SetParameterValue("@PayId", orderPayModel.PayId);
                cmd.SetParameterValue("@Status", orderPayModel.Status);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
