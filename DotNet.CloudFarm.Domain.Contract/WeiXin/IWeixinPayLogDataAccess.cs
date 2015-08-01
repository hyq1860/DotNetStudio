using DotNet.CloudFarm.Domain.Model.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Contract.WeiXin
{
    /// <summary>
    /// 微信支付日志
    /// </summary>
    public interface IWeixinPayLogDataAccess
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="weixinPayLog"></param>
        /// <returns></returns>
        int Insert(WeixinPayLog weixinPayLog);

    }
}
