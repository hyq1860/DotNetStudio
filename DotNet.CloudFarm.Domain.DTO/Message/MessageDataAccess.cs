using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
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

        public PagedList<MessageModel> GetMessages(int userId, int pageIndex, int pageSize)
        {
            var data = new List<MessageModel>();
            
            using (var cmd = DataCommandManager.GetDataCommand("GetMessages"))
            {
                cmd.SetParameterValue("@UserId", userId);
                cmd.SetParameterValue("@PageIndex", pageIndex);
                cmd.SetParameterValue("@PageSize", pageSize);
                var total = 0;
                using (var dr=cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        var messageModel = new MessageModel();
                        if (total == 0)
                        {
                            total = !Convert.IsDBNull(dr["Total"]) ? Convert.ToInt32(dr["Total"]) : 0;
                        }
                        messageModel.MessageId = !Convert.IsDBNull(dr["Id"]) ? Convert.ToInt32(dr["Id"]) : 0;
                        messageModel.UserId = !Convert.IsDBNull(dr["UserId"]) ? Convert.ToInt32(dr["UserId"]) : 0;
                        messageModel.Content = !Convert.IsDBNull(dr["MsgContent"]) ? dr["MsgContent"].ToString() : string.Empty;
                        messageModel.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
                        messageModel.Status = !Convert.IsDBNull(dr["Status"]) ? Convert.ToInt32(dr["Status"]) : 0;
                        if (messageModel.MessageId > 0)
                        {
                            data.Add(messageModel);
                        }
                    }
                }
                var result = new PagedList<MessageModel>(data, pageIndex, pageSize)
                {
                    TotalCount = total
                };
                return result;
            }
            
        }

        public bool UpdateMessageStatus(int userId)
        {
            using (var cmd = DataCommandManager.GetDataCommand("UpdateMessageStatus"))
            {
                cmd.SetParameterValue("@Status", 1);
                cmd.SetParameterValue("@UserId", userId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
