using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.Message;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.Common.Collections;
using DotNet.Common.Models;
using DotNet.Data;

namespace DotNet.CloudFarm.Domain.DTO.Message
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageDataAccess : IMessageDataAccess
    {
        public Result<MessageModel> SendSms(int userId, string content)
        {
            var result = new Result<MessageModel>();
            using (var cmd = DataCommandManager.GetDataCommand("InsertMessage"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@MsgContent", content);
                cmd.SetParameterValue("@CreateTime", DateTime.Now);
                cmd.SetParameterValue("@Status", 0);
                result.Status=new Status();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result.Status.Code = "1";
                }
                else
                {
                    result.Status.Code = "0";
                }
            }
            return result;
        }

        public Result<PagedList<MessageModel>> GetMessages(int userId, int pageIndex, int pageSize)
        {
            var result = new Result<PagedList<MessageModel>>();
            result.Data = new PagedList<MessageModel>(new List<MessageModel>(), pageIndex, pageSize);
            return result;
        }
    }
}
