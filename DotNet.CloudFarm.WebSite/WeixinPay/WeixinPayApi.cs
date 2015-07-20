using log4net;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;

namespace DotNet.CloudFarm.WebSite.WeixinPay
{
    /// <summary>
    /// 微信支付接口
    /// by lg 2015-7-17
    /// </summary>
    public class WeixinPayApi
    {

        /// <summary>
        /// 与微信公众账号后台的Token设置保持一致，区分大小写。
        /// </summary>
        public static readonly string Token = WebConfigurationManager.AppSettings["WeixinToken"];
        /// <summary>
        ///与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
        public static readonly string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];
        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];
        /// <summary>
        /// 与微信公众账号后台的AppSecret设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        /// <summary>
        /// 微信商户号
        /// </summary>
        public static readonly string Mchid = WebConfigurationManager.AppSettings["WeixinMchid"];
        /// <summary>
        /// 微信支付KEY
        /// </summary>
        public static readonly string PayKey = WebConfigurationManager.AppSettings["WeixinPaySecretKey"];
        /// <summary>
        /// 证书存放地址
        /// </summary>
        public static readonly string SSLCERT_PATH = WebConfigurationManager.AppSettings["WeixinSSLCERT_PATH"];
        /// <summary>
        /// 证书密码
        /// </summary>
        public static readonly string SSLCERT_PASSWORD = WebConfigurationManager.AppSettings["WeixinSSLCERT_PASSWORD"];

        /// <summary>
        /// 微信支付使用IP(当前请求IP)
        /// </summary>
        public static readonly string IP = WebConfigurationManager.AppSettings["WeixinIP"];

        /// <summary>
        /// 微信支付回调地址
        /// </summary>
        public static readonly string NotifyUrl = WebConfigurationManager.AppSettings["WexinPayNotifyUrl"];

        private static ILog logger = LogManager.GetLogger("WeixnPayApi");
        /// <summary>
        /// 企业支付
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">金额</param>
        /// <param name="desc">付款描述信息</param>
        /// <returns></returns>
        public static string QYPay(string openId,long orderId,decimal amount,string desc)
        {
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            var nonceStr = TenPayV3Util.GetNoncestr();

            //创建请求接口参数
            packageReqHandler.SetParameter("mch_appid", AppId);
            packageReqHandler.SetParameter("mchid", Mchid);
            packageReqHandler.SetParameter("nonce_str", nonceStr);
            packageReqHandler.SetParameter("partner_trade_no", orderId.ToString());
            packageReqHandler.SetParameter("openid", openId);
            packageReqHandler.SetParameter("check_name", "NO_CHECK");//不校验用户姓名
            packageReqHandler.SetParameter("desc", desc);
            packageReqHandler.SetParameter("amount", Convert.ToInt32(amount * 100).ToString());
            packageReqHandler.SetParameter("spbill_create_ip", IP );//TODO:替换成可配置文件
            string sign = packageReqHandler.CreateMd5Sign("key", PayKey);
            packageReqHandler.SetParameter("sign", sign);

            string data = packageReqHandler.ParseXML();

            //证书相关
            var cert = new X509Certificate2(SSLCERT_PATH, SSLCERT_PASSWORD);

            try
            {
                //调用统一订单接口
                var result = TenPayV3.QYPay(data, cert);
                logger.Info(result);
                var unifiedorderRes = XDocument.Parse(result);
                string return_code = unifiedorderRes.Element("xml").Element("return_code").Value;
                return return_code;
            }
            catch (Exception e)
            {
                logger.Error(e);
                return "ERROR";
            }
        }

        /// <summary>
        /// 微信支付接口
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">金额</param>
        /// <param name="createIp">用户IP</param>
        /// <param name="openId">用户OpenId</param>
        /// <returns>prepay_id:微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时; </returns>
        public static string Unifiedorder(string productName, long orderId, decimal amount, string createIp,string openId)
        {
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);

            var nonceStr = TenPayV3Util.GetNoncestr();

            //创建请求统一订单接口参数
            packageReqHandler.SetParameter("appid", AppId);
            packageReqHandler.SetParameter("mch_id", Mchid);
            packageReqHandler.SetParameter("nonce_str", nonceStr);
            packageReqHandler.SetParameter("body", productName);
            packageReqHandler.SetParameter("out_trade_no", orderId.ToString());
            packageReqHandler.SetParameter("total_fee", Convert.ToInt32(amount * 100).ToString());
            packageReqHandler.SetParameter("spbill_create_ip", createIp);
            packageReqHandler.SetParameter("notify_url", NotifyUrl);
            packageReqHandler.SetParameter("trade_type", "JSAPI");
            packageReqHandler.SetParameter("openid", openId);
            string sign = packageReqHandler.CreateMd5Sign("key", PayKey);
            packageReqHandler.SetParameter("sign", sign);

            string data = packageReqHandler.ParseXML();

            //证书相关
            var cert = new X509Certificate2(SSLCERT_PATH, SSLCERT_PASSWORD);
              try
            {
                //调用统一订单接口
                var result = TenPayV3.Unifiedorder(data, cert);
                logger.Info("同一订单接口返回信息："+result);
                var unifiedorderRes = XDocument.Parse(result);
                string return_code = unifiedorderRes.Element("xml").Element("return_code").Value;
                string result_code =unifiedorderRes.Element("xml").Element("result_code").Value;
                if (return_code == "SUCCESS" && result_code == "SUCCESS")
                    return unifiedorderRes.Element("xml").Element("prepay_id").Value;
                else
                    return "FAIL";
            }
            catch (Exception e)
            {
                logger.Error(e);
                return "ERROR";
            }
        }
    }
}