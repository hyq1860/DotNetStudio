using DotNet.CloudFarm.Domain.Contract.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Data;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using System.Data;

namespace DotNet.CloudFarm.Domain.DTO.WeiXin
{
    /// <summary>
    /// 微信消息
    /// </summary>
    public class WeiXinMessageDataAccess :IWeiXinMessageDataAccess
    {
        /// <summary>
        /// 增加自动回复消息
        /// </summary>
        /// <param name="message">message实体</param>
        public int AddAutoReplyMessage(WeixinAutoReplyMessageModel message)
        {
            using (var cmd = DataCommandManager.GetDataCommand("AutoReplyMessage_Add"))
            {
                cmd.SetParameterValue("@Keyword", message.Keyword);
                cmd.SetParameterValue("@ReplyContent", message.ReplyContent);
                cmd.SetParameterValue("@CreatorId", message.CreatorId);
                cmd.SetParameterValue("@Status", message.Status);
                cmd.SetParameterValue("@CreateTime", message.CreateTime);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }

        public void UpdateAutoReplyMessageStatus(int id,int status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有可用的自动回复关键字对象
        /// </summary>
        /// <returns></returns>
        public IList<WeixinAutoReplyMessageModel> GetAllAutoReplyMessage()
        {
            var list = new List<WeixinAutoReplyMessageModel>();
            using (var cmd = DataCommandManager.GetDataCommand("AutoReplyMessage_GetAll"))
            {
                using (var dt = cmd.ExecuteDataTable())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(convertRowToWEiXinAutoReplyMessageModel(dr));
                    }

                }
            }
            return list;
        }
        /// <summary>
        /// 转化方法
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private WeixinAutoReplyMessageModel convertRowToWEiXinAutoReplyMessageModel(DataRow dr)
        {
            var model = new WeixinAutoReplyMessageModel();
            model.Id = !Convert.IsDBNull(dr["Id"]) ? int.Parse(dr["Id"].ToString()) : 0;
            model.Keyword = !Convert.IsDBNull(dr["Keyword"]) ? dr["Keyword"].ToString() : string.Empty;
            model.ReplyContent = !Convert.IsDBNull(dr["ReplyContent"]) ? dr["ReplyContent"].ToString() : string.Empty;
            model.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
            model.CreatorId = !Convert.IsDBNull(dr["CreatorId"]) ? int.Parse(dr["CreatorId"].ToString()) : 0;
            model.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
            return model;

        }
    }
}
