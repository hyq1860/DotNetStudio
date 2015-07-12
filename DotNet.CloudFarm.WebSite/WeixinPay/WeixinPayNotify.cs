using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.CloudFarm.WebSite.WeixinPay
{
    public class WeixinPayNotify:Notify
    {
        public WeixinPayNotify(HttpContextBase httpContextBase)
            : base(httpContextBase)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                httpContextBase.Response.Write(res.ToXml());
                httpContextBase.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            ////查询订单，判断订单真实性
            //if (!QueryOrder(transaction_id))
            //{
            //    //若订单查询失败，则立即返回结果给微信支付后台
            //    WxPayData res = new WxPayData();
            //    res.SetValue("return_code", "FAIL");
            //    res.SetValue("return_msg", "订单查询失败");
            //    Log.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
            //    page.Response.Write(res.ToXml());
            //    page.Response.End();
            //}
            ////查询订单成功
            //else
            //{
            //    WxPayData res = new WxPayData();
            //    res.SetValue("return_code", "SUCCESS");
            //    res.SetValue("return_msg", "OK");
            //    Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
            //    page.Response.Write(res.ToXml());
            //    page.Response.End();
            //}
        }
    }
}