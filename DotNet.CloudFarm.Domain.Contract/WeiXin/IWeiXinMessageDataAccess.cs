﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.WeiXin;

namespace DotNet.CloudFarm.Domain.Contract.WeiXin
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public interface IWeiXinMessageDataAccess
    {
        /// <summary>
        /// 增加自动回复消息
        /// </summary>
        /// <param name="message">message实体</param>
        int AddAutoReplyMessage(WeixinAutoReplyMessageModel message);

        /// <summary>
        /// 删除自动回复消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdateAutoReplyMessageStatus(int id,int status);

        /// <summary>
        /// 获取全部自动回复消息（未删除的）
        /// </summary>
        /// <returns></returns>
        IList<WeixinAutoReplyMessageModel> GetAllAutoReplyMessage();


        /// <summary>
        /// 检查keyword是否存在
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        bool CheckKeyword(string keyword);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void Update(WeixinAutoReplyMessageModel model);

        /// <summary>
        /// 根据keword获取实体
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        WeixinAutoReplyMessageModel GetAutoReplyMessageByKeyword(string keyword);
    }
}
