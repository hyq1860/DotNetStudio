﻿using System;
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
    }
}