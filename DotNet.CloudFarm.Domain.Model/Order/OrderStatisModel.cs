using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Order
{
    public class OrderStatisModel
    {
        public OrderStatisModel()
        {
            UserOrderList=new List<UserOrderModel>();
        }

        public int TotalUserCount { get; set; }

        public List<UserOrderModel> UserOrderList { get; set; }
    }

    public class UserOrderModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int RowId { get; set; }

        public int UserId { get; set; }

        public decimal TotalMoney { get; set; }
    }
}
