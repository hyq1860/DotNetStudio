using DotNet.CloudFarm.Domain.Contract.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using DotNet.Data;
using System.Data;

namespace DotNet.CloudFarm.Domain.DTO.WeiXin
{
    public class WeixinPayLogDataAccess:IWeixinPayLogDataAccess
    {
        public int Insert(WeixinPayLog weixinPayLog)
        {
            using (var cmd = DataCommandManager.GetDataCommand("WeixinPayLogInsert"))
            {
                cmd.SetParameterValue("@OrderId", weixinPayLog.OrderId);
                cmd.SetParameterValue("@WxOpenId", weixinPayLog.WxOpenId);
                cmd.SetParameterValue("@Amount", weixinPayLog.Amount);
                cmd.SetParameterValue("@Description", weixinPayLog.Description);
                cmd.SetParameterValue("@Status", weixinPayLog.Status);
                cmd.SetParameterValue("@CreateTime", weixinPayLog.CreateTime);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }


        public IList<WeixinPayLog> GetListByOrderId(long orderId)
        {
            var list = new List<WeixinPayLog>();
            using (var cmd = DataCommandManager.GetDataCommand("WexinPayLogGetByOrderId"))
            {
                cmd.SetParameterValue("@OrderId",orderId);
                using (var dt = cmd.ExecuteDataTable())
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(convertToWeixinPayLog(dr));
                    }

                }
            }
            return list;
        }

        private WeixinPayLog convertToWeixinPayLog(DataRow dr)
        {
            var model = new WeixinPayLog();
            model.Id = !Convert.IsDBNull(dr["Id"]) ? int.Parse(dr["Id"].ToString()) : 0;
            model.WxOpenId = !Convert.IsDBNull(dr["WxOpenId"]) ? dr["WxOpenId"].ToString() : string.Empty;
            model.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
            model.OrderId = !Convert.IsDBNull(dr["OrderId"]) ? long.Parse(dr["OrderId"].ToString()) : 0;
            model.Amount = !Convert.IsDBNull(dr["Amount"]) ?  decimal.Parse(dr["Amount"].ToString()) : 0;
            model.Description = !Convert.IsDBNull(dr["Description"]) ? dr["Description"].ToString() : string.Empty;
            model.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
            return model;
        }


        public WeixinPayLog GetPayLogById(int id)
        {
            var paylog = new WeixinPayLog();
            using (var cmd = DataCommandManager.GetDataCommand("WexinPayLogGetById"))
            {
                cmd.SetParameterValue("@Id",id);
                using (var dr = cmd.ExecuteDataReader())
                {
                    while (dr.Read())
                    {
                        paylog = convertToWeixinPayLog(dr);
                    }
                }
               
            }
            return paylog;

        }

        /// <summary>
        /// 检查是否存在未成功的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status">状态</param>
        /// <returns>true:该状态下还存在数据,false:该状态下不存在数据</returns>
        public bool WeixinPayLogCheckStatus(long orderId, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("WeixinPayLogCheckStatus"))
            {
                cmd.SetParameterValue("@OrderId", orderId);
                cmd.SetParameterValue("@Status", status);
                return cmd.ExecuteScalar<int>() > 0;
            }
        }
        /// <summary>
        /// 更新支付日志状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool WeixinPayLogUpdateStatus(int id, int status)
        {
            using (var cmd = DataCommandManager.GetDataCommand("WeixinPayLogUpdateStatus"))
            {
                cmd.SetParameterValue("@Id", id);
                cmd.SetParameterValue("@Status", status);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private WeixinPayLog convertToWeixinPayLog(IDataReader dr)
        {
            var model = new WeixinPayLog();
            model.Id = !Convert.IsDBNull(dr["Id"]) ? int.Parse(dr["Id"].ToString()) : 0;
            model.WxOpenId = !Convert.IsDBNull(dr["WxOpenId"]) ? dr["WxOpenId"].ToString() : string.Empty;
            model.Status = !Convert.IsDBNull(dr["Status"]) ? int.Parse(dr["Status"].ToString()) : 0;
            model.OrderId = !Convert.IsDBNull(dr["OrderId"]) ? long.Parse(dr["OrderId"].ToString()) : 0;
            model.Amount = !Convert.IsDBNull(dr["Amount"]) ? decimal.Parse(dr["Amount"].ToString()) : 0;
            model.Description = !Convert.IsDBNull(dr["Description"]) ? dr["Description"].ToString() : string.Empty;
            model.CreateTime = !Convert.IsDBNull(dr["CreateTime"]) ? Convert.ToDateTime(dr["CreateTime"]) : DateTime.MinValue;
            return model;
        }


        public int WeixinUserInsert(WeixinUser weixinUser)
        {
            using (var cmd = DataCommandManager.GetDataCommand("WeixinUserInsert"))
            {
                cmd.SetParameterValue("@openid", weixinUser.openid);
                cmd.SetParameterValue("@nickname", weixinUser.nickname);
                cmd.SetParameterValue("@headimgurl", weixinUser.headimgurl);
                cmd.SetParameterValue("@createTime", weixinUser.createtime);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
        }
    }
}
