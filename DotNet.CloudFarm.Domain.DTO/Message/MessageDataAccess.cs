using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Message;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.DTO.Message
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageDataAccess : IMessageDataAccess
    {
        public Result<MessageModel> SendSms(int userId, string content)
        {
            throw new NotImplementedException();
        }

        public Result<PagedList<MessageModel>> GetMessages(int userId, int pageIndex, int pageSize, int status)
        {
            throw new NotImplementedException();
        }
    }
}
