using DotNet.CloudFarm.Domain.Contract.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using DotNet.Data;

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
    }
}
