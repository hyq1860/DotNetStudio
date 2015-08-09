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

        /// <summary>
        /// 根据orderid获取支付日志
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<WeixinPayLog> GetListByOrderId(long orderId);

        /// <summary>
        /// 根据ID获取支付日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WeixinPayLog GetPayLogById(int id);
        /// <summary>
        /// 检查是否存在未成功的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status">状态</param>
        /// <returns>true:该状态下还存在数据,false:该状态下不存在数据</returns>
        bool WeixinPayLogCheckStatus(long orderId, int status);

        /// <summary>
        /// 更新支付日志状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool WeixinPayLogUpdateStatus(int id, int status);

        /// <summary>
        /// 插入微信用户信息
        /// </summary>
        /// <param name="weixinUser"></param>
        /// <returns></returns>
        int WeixinUserInsert(WeixinUser weixinUser);
    }
}
