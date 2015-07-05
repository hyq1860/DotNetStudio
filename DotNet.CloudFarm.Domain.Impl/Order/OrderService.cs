
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.Order;
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

        public PagedList<OrderModel> GetOrderList(int userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Result<OrderModel> SubmitOrder(OrderModel orderModel)
        {
            throw new NotImplementedException();
        }

        public List<TopOrderInfo> GetTopOrderList(int top, int pageIndex, int pageSize)
        {
            return orderDataAccess.GetTopOrderList(top, pageIndex, pageSize);
        }
    }
}
