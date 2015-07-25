using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Contract.Message
{
    public interface IMessageService
    {
        /// <summary>
        /// 发送短消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Result<MessageModel> SendSms(int userId,string content);

        /// <summary>
        /// 获取消息分页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Result<PagedList<MessageModel>> GetMessages(int userId,int pageIndex,int pageSize);

        /// <summary>
        /// 设置消息状态
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UpdateMessageStatus(int userId);

        /// <summary>
        /// 检查是否有未读短消息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool CheckUnreadMessage(int userId);
    }
}
