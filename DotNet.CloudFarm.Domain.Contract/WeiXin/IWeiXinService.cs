using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.WeiXin;
namespace DotNet.CloudFarm.Domain.Contract.WeiXin
{
    /// <summary>
    /// 微信相关业务功能
    /// </summary>
    public interface IWeiXinService
    {
        /// <summary>
        /// 插入message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        int AutoReplyMessageInsert(WeixinAutoReplyMessageModel message);

        /// <summary>
        /// 获取所有自动回复关键字数据(未删除)
        /// </summary>
        /// <returns></returns>
        IList<WeixinAutoReplyMessageModel> AutoReplyMessageGetAll();

        /// <summary>
        /// 检查keyword是否存在
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        bool AutoReplyMessageCheckKeyword(string keyword);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void AutoReplyMessageUpdate(WeixinAutoReplyMessageModel model);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void AutoReplyMessageUpdateStatus(int id, int status);

        /// <summary>
        /// 根据关键字获取回复信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        WeixinAutoReplyMessageModel AutoReplyMessageGetByKeyword(string keyword);

        /// <summary>
        /// 插入微信支付日志
        /// </summary>
        /// <param name="weixinPayLog"></param>
        /// <returns></returns>
        int InsertWeixinPayLog(WeixinPayLog weixinPayLog);

        /// <summary>
        /// 根据订单号获取支付日志
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<WeixinPayLog> GetPayLogListByOrderId(long orderId);

        /// <summary>
        /// 根据ID获取支付日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WeixinPayLog GetPayLogById(int id);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        bool WeixinPayLogUpdateStatus(int id, int status);

        /// <summary>
        /// 检查是否存在未成功的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status">状态</param>
        /// <returns>true:该状态下还存在数据,false:该状态下不存在数据</returns>
        bool WeixinPayLogCheckStatus(long orderId, int status);

        /// <summary>
        /// 插入微信用户
        /// </summary>
        /// <param name="weixinUser"></param>
        /// <returns></returns>
        int WeixinUserInsert(WeixinUser weixinUser);

    }
}
