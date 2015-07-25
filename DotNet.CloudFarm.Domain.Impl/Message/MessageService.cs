using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.CloudFarm.Domain.Contract.Message;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl.Message
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public class MessageService:IMessageService
    {
        
        private IMessageDataAccess messageDataAccess;

        public MessageService(IMessageDataAccess messageDataAccess)
        {
            this.messageDataAccess = messageDataAccess;
        }

        public Result<MessageModel> SendSms(int userId, string content)
        {
            return messageDataAccess.SendSms(userId, content);
        }

        public Result<PagedList<MessageModel>> GetMessages(int userId, int pageIndex, int pageSize)
        {
            var result = new Result<PagedList<MessageModel>>
            {
                Data = messageDataAccess.GetMessages(userId, pageIndex, pageSize),
                Status = new Status() { Code="1",Message = ""}
            };
            return result; 
        }

        public bool UpdateMessageStatus(int userId)
        {
            return messageDataAccess.UpdateMessageStatus(userId);
        }
    }
}
