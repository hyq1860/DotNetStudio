using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP;
using System.IO;
using System.Web.Configuration;
using Senparc.Weixin.MP.MvcExtension;
using DotNet.CloudFarm.WebSite.Models;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities.Menu;
using log4net;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// 微信接收相关Controller
    /// </summary>
    public class WeixinController : BaseController
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

        public static readonly string SSLCERT_PATH = WebConfigurationManager.AppSettings["WeixinSSLCERT_PATH"];

        public static readonly string SSLCERT_PASSWORD = WebConfigurationManager.AppSettings["WeixinSSLCERT_PASSWORD"];
        private ILog logger = LogManager.GetLogger("WeiXinController");

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;
            postModel.AppId = AppId;

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, maxRecordCount);
            try
            {

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;
                
                 
                //执行微信处理过程
                messageHandler.Execute();

                //测试时可开启，帮助跟踪数据

                //if (messageHandler.ResponseDocument == null)
                //{
                //    throw new Exception(messageHandler.RequestDocument.ToString());
                //}

                if (messageHandler.ResponseDocument != null)
                {
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                if (messageHandler.UsingEcryptMessage)
                {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                return new WeixinResult(messageHandler);//v0.8+
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Content("");
            }
        }

        /// <summary>
        /// 微信支付回调地址
        /// </summary>
        /// <returns></returns>
        public ContentResult PayNotify()
        {
            return Content("1");
        }

        /// <summary>
        /// 测试创建菜单的部分
        /// </summary>
        /// <param name="MemuName"></param>
        /// <returns></returns>
        public ActionResult CreateMenuTest()
        {
            var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);

            //单击
            var btnGroup = new ButtonGroup();
            btnGroup.button.Add(new SingleClickButton()
                {
                    name = "单击测试",
                    key = "OnClick",
                    type = ButtonType.click.ToString()
                });

            //二级菜单
            var subButton = new SubButton()
            {
                name = "二级菜单"
            };
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_Text",
                name = "返回文本"
            });
            subButton.sub_button.Add(new SingleClickButton()
            {
                key = "SubClickRoot_News",
                name = "返回图文"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/weixin/pay",
                name = "付"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/",
                name = "Url跳转"
            });
            btnGroup.button.Add(subButton);
            var result = CommonApi.CreateMenu(accesstoken, btnGroup);
            return Json(result,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 测试获取用户信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public ActionResult GetUserTest(string openid)
        {
            var accessToken = AccessTokenContainer.TryGetToken(AppId, AppSecret);
            var result = CommonApi.GetUserInfo(accessToken, openid);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 企业支付TEST
        /// </summary>
        /// <param name="openId">openId</param>
        /// <param name="orderId">订单号</param>
        /// <param name="amount">金额</param>
        /// <param name="desc">付款描述信息</param>
        /// <returns></returns>
        public ContentResult QyPayTest(string openId,string orderId,decimal amount,string desc)
        {
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);

            var sp_billno = DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);
            var nonceStr = TenPayV3Util.GetNoncestr();

            //创建请求统一订单接口参数
            packageReqHandler.SetParameter("mch_appid", AppId);
            packageReqHandler.SetParameter("mchid", Mchid);
            packageReqHandler.SetParameter("nonce_str", nonceStr);
            packageReqHandler.SetParameter("partner_trade_no", orderId);
            packageReqHandler.SetParameter("openid", openId);
            packageReqHandler.SetParameter("check_name", "NO_CHECK");//不校验用户姓名
            packageReqHandler.SetParameter("desc", desc);
            packageReqHandler.SetParameter("amount", (amount * 100).ToString());
            packageReqHandler.SetParameter("spbill_create_ip", "101.200.233.5");//TODO:替换成可配置文件
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
                return Content(return_code);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return Content("-1");
            }

        }

        public ActionResult Pay()
        {
            var openId = "oOGootzpwe38CkQSTj00wyHhKSMk";
             var timeStamp = TenPayV3Util.GetTimestamp();
               var nonceStr = TenPayV3Util.GetNoncestr();
               var sp_billno = Convert.ToInt64(timeStamp);
             var pre_id = WeixinPay.WeixinPayApi.Unifiedorder("测试用", sp_billno, 0.01M, Request.UserHostAddress, openId);
             if (pre_id == "ERROR" && pre_id == "FAIL")
                 return Content("ERROR");
             var package = "prepay_id=" + pre_id;
             ViewBag.Package = package;
             ViewBag.AppId = AppId;
          
             var req = new RequestHandler(null);
             req.SetParameter("appId", AppId);
             req.SetParameter("timeStamp", timeStamp);
             req.SetParameter("package", package);
             req.SetParameter("signType", "MD5");
             var paySign =req.CreateMd5Sign("key", PayKey);
             ViewBag.TimeStamp = timeStamp;
             ViewBag.NonceStr = nonceStr;
             ViewBag.PaySign = paySign;
            return View();
        }
    }
}