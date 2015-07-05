using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Collections;
using DotNet.Common.Models;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.DTO.Order
{
    public class OrderDataAccess:IOrderDataAccess
    {
        public List<TopOrderInfo> GetTopOrderList(int top, int pageIndex, int pageSize)
        {
            var result = new List<TopOrderInfo>();
            using (var cmd = DataCommandManager.GetDataCommand(""))
            {
                using (var dr=cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var topOrderInfo = new TopOrderInfo();
                        topOrderInfo.UserId = !Convert.IsDBNull(dr["ID"]) ? int.Parse(dr["ID"].ToString()) : 0;
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
        public Result<OrderModel> SubmitOrder(OrderModel orderModel)
        {
            var result = new Result<OrderModel>();
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
                    
                }
            }
            return result;
        }

        public Result<OrderModel> GetOrder(long orderId, int userId)
        {
            var result = new Result<OrderModel>();
            using (var cmd = DataCommandManager.GetDataCommand("GetOrder"))
            {
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var orderModel = new OrderModel();

                        orderModel.OrderId = !Convert.IsDBNull("OrderId") ? long.Parse(dr["OrderId"].ToString()) : 0;
                        orderModel.UserId = !Convert.IsDBNull("UserId") ? int.Parse(dr["UserId"].ToString()) : 0;
                        orderModel.ProductId = !Convert.IsDBNull("ProductId") ? int.Parse(dr["ProductId"].ToString()) : 0;
                        orderModel.ProductCount = !Convert.IsDBNull("ProductCount") ? int.Parse(dr["ProductCount"].ToString()) : 0;
                        orderModel.Price = !Convert.IsDBNull("Price") ? long.Parse(dr["Price"].ToString()) : 0;
                        orderModel.Status = !Convert.IsDBNull("Status") ? int.Parse(dr["Status"].ToString()) : 0;
                        orderModel.PayType = !Convert.IsDBNull("PayType") ? int.Parse(dr["PayType"].ToString()) : 0;
                        orderModel.CreateTime = !Convert.IsDBNull("CreateTime") ? DateTime.Parse(dr["CreateTime"].ToString()) : DateTime.MinValue;
                        
                        result.Data = orderModel;
                    }
                }
            }

            return result;
        }

        public PagedList<OrderModel> GetOrderList(int userId, int pageIndex, int pageSize)
        {
            var result = new PagedList<OrderModel>(new List<OrderModel>(), pageIndex,pageSize);

            return result;
        }
    }
}
