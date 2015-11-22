using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model;
using DotNet.CloudFarm.Domain.Model.Product;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    public class PreSaleOrderViewModel
    {
        public PreSaleOrderViewModel()
        {
            Provinces=new List<Address>();
            Cities = new List<Address>();
            Areas = new List<Address>();
        }

        public PreSaleProduct PreSaleProduct { get; set; }

        public List<Address> Provinces { get; set; } 

        public List<Address> Cities { get; set; } 

        public List<Address> Areas { get; set; } 


        public string ProvinceId { get; set; }

        public string CityId { get; set; }

        public string AreaId { get; set; }

        #region 微信支付先关
        public string TimeStamp { get; set; }

        public string NonceStr { get; set; }

        public string PaySign { get; set; }

        public string Package { get; set; }

        public string AppId { get; set; }
        #endregion

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }

        public decimal TotalMoney { get; set; }
    }
}
