using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl.Order
{
    public class PreSaleOrderService:IPreSaleOrderService
    {
        private IPreSaleOrderDataAccess preSaleOrderDataAccess;
        public PreSaleOrderService(IPreSaleOrderDataAccess preSaleOrderDataAccess)
        {
            this.preSaleOrderDataAccess = preSaleOrderDataAccess;
        }

        public PagedList<PreSaleOrder> GetPreSaleOrderCollection(Expression<Func<PreSaleOrder, bool>> whereFunc, Expression<Func<PreSaleOrder, long>> orderByFunc, string orderType,int pageIndex,int pageSize)
        {
            var all = preSaleOrderDataAccess.GetList();
            var condition = all.OrderBy(orderByFunc);
            var total = condition.Count();
            var data = condition.Skip(pageSize*(pageIndex - 1)).Take(pageSize).ToList();
            var result = new PagedList<PreSaleOrder>(data, pageIndex, pageSize, total);
            return result;
        }

        public Result<PagedList<PreSaleOrder>> GetPreSaleOrderList(Expression<Func<PreSaleOrder, bool>> whereFunc,int pageIndex, int pageSize)
        {
            var pagedPreOrder = preSaleOrderDataAccess.GetPagedList(whereFunc, p => p.OrderId, "desc", pageIndex, pageSize).ToList();
            foreach (var order in pagedPreOrder)
            {
                if (order.Status != 2)
                {
                    order.ExpressDelivery = "暂无";
                }
            }
            var result = new Result<PagedList<PreSaleOrder>>();
            result.Data = new PagedList<PreSaleOrder>(pagedPreOrder,pageIndex,pageSize);
            return result;
        }

        public int SubmitPreSaleOrder(PreSaleOrder preSaleOrder)
        {
            return preSaleOrderDataAccess.Add(preSaleOrder);
        }

        public Result<PagedList<PreSaleOrder>> GetPreSaleOrderList(int userId, int pageIndex, int pageSize)
        {
            var result=new Result<PagedList<PreSaleOrder>>();
            var total = preSaleOrderDataAccess.GetList().Count(p => p.UserId == userId);
            var data =
                preSaleOrderDataAccess.GetPagedList(p => p.UserId == userId, p => p.OrderId, "desc", pageIndex, pageSize)
                    .ToList();
            var pagedList=new PagedList<PreSaleOrder>(data,pageIndex,pageSize, total);
            result.Data = pagedList;
            return result;
        }

        public bool ModifyPreOrder(PreSaleOrder preSaleOrder)
        {
            var order= preSaleOrderDataAccess.GetById(preSaleOrder.OrderId);
            if (order != null)
            {
                order.Status = preSaleOrder.Status;
                if (!string.IsNullOrEmpty(preSaleOrder.ExpressDelivery))
                {
                    order.ExpressDelivery = preSaleOrder.ExpressDelivery;
                }
                order.ModifyTime=DateTime.Now;
                return preSaleOrderDataAccess.Update(order) > 0;
            }
            return false;
        }

        public PreSaleOrder GetPreSaleOrder(long orderId)
        {
            return preSaleOrderDataAccess.GetById(orderId);
        }
    }
}
