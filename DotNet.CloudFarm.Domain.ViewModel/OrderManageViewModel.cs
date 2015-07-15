using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    public class OrderManageViewModel:OrderViewModel
    {
        /// <summary>
        /// 下单用户手机号
        /// </summary>
        public string Mobile { get; set; }

    }
}
