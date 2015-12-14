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
using System.Text;
using System.Collections;
using Senparc.Weixin.MP.Helpers;
using DotNet.CloudFarm.Domain.Contract.Order;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;
using DotNet.CloudFarm.Domain.Impl.Order;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.Common.Utility;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using DotNet.CloudFarm.Domain.Contract.WeiXin;

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

        [Ninject.Inject]
        public IOrderService OrderService { get; set; }

        [Ninject.Inject]
        public IWeiXinService WeixinService { get; set; }


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

            var btnGroup = new ButtonGroup();
            //二级菜单
            var subButton = new SubButton()
            {
                name = "羊客商城"
            };
            subButton.sub_button.Add(new SingleViewButton() {
                url="http://yk.kerchinsheep.com/home/presaleproduct",
                name = "羊客商城"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/home/PreSaleOrderList",
                name = "商城订单"
            });
            subButton.sub_button.Add(new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/html/recipeMain.html",
                name = "羊客食谱"
            });
            btnGroup.button.Add(subButton);
            var clickButton = new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/",
                name = "羊羊得益"
            };
            btnGroup.button.Add(clickButton);
            var subButton1 = new SubButton()
            { 
              name="更多精彩"
            };
            subButton1.sub_button.Add(new SingleViewButton()
            {
                url = "http://yk.kerchinsheep.com/html/micro.html",
                name = "羊客介绍"
            });
            subButton1.sub_button.Add(new SingleClickButton()
            {
                key = "learnmore",
                name = "了解更多"
            });
           
            var clickButton1 = new SingleViewButton()
            {
                url = "http://activity.kerchinsheep.com/app/h5/zy",
                name = "全民套羊"
            };
            subButton1.sub_button.Add(clickButton1);
            btnGroup.button.Add(subButton1);
            var result = CommonApi.CreateMenu(accesstoken, btnGroup);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// 测试临时菜单的部分
        /// </summary>
        /// <param name="MemuName"></param>
        /// <returns></returns>
        //public ActionResult CreateMenuTemp()
        //{
        //    var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);

        //    var btnGroup = new ButtonGroup();
        //    //二级菜单
        //    var subButton = new SubButton()
        //    {
        //        name = "关于羊客"
        //    };
        //    subButton.sub_button.Add(new SingleClickButton()
        //    {
        //        key = "updating",
        //        name = "微场景"
        //    });
        //    subButton.sub_button.Add(new SingleClickButton()
        //    {
        //        key = "updating",
        //        name = "了解更多"
        //    });
        //    btnGroup.button.Add(subButton);
        //    var clickButton = new SingleClickButton()
        //    {
        //        key = "updating",
        //        name = "羊羊得益"
        //    };
        //    btnGroup.button.Add(clickButton);
        //    var result = CommonApi.CreateMenu(accesstoken, btnGroup);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 测试获取所有关注用户
        /// </summary>
        /// <returns></returns>
        //public ContentResult GetAllUser()
        //{
        //    try
        //    {
        //        var appid = "wxe9fb1463b624aa89";
        //        var appsecret = "7e0715ade8eaac0f533a6f2d67a7b846 ";
        //        var nextId = "";
        //        var accesstoken = AccessTokenContainer.TryGetToken(appid, appsecret);

        //        var openidResultJson = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Get(accesstoken, nextId);
        //        nextId = openidResultJson.next_openid;
        //        foreach (var openId in openidResultJson.data.openid)
        //        {
        //            var open = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Info(accesstoken, openId);
        //            var weixinuser = new WeixinUser()
        //            {
        //                openid = open.openid,
        //                headimgurl = open.headimgurl,
        //                nickname = open.nickname,
        //                createtime = DateTime.Now
        //            };
        //            WeixinService.WeixinUserInsert(weixinuser);
        //        }

        //        while (string.IsNullOrEmpty(nextId))
        //        {
        //            openidResultJson = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Get(accesstoken, nextId);
        //            nextId = openidResultJson.next_openid;
        //            foreach (var openId in openidResultJson.data.openid)
        //            {
        //                var open = Senparc.Weixin.MP.AdvancedAPIs.User.UserApi.Info(accesstoken, openId);
        //                var weixinuser = new WeixinUser()
        //                {
        //                    openid = open.openid,
        //                    headimgurl = open.headimgurl,
        //                    nickname = open.nickname
        //                };
        //                WeixinService.WeixinUserInsert(weixinuser);

        //            }
        //        }
        //        return Content("done");
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        return Content("error");
        //    }
           
        //}


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
        /// <summary>
        /// 按订单号进行支付
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public ActionResult Pay(long orderid=81150718819501)
        {
            //TODO:将该页加入登录页,就可以启用下边的注释
            //var userid = UserInfo.UserId;
            //var openId = UserInfo.WxOpenId;
            var userid = 161;
            var openId = "oOGootzpwe38CkQSTj00wyHhKSMk";
            var order = OrderService.GetOrderViewModel(userid, orderid);
            if(string.IsNullOrEmpty(order.ProductName) || order.OrderId==0 || order.TotalMoney==0M)
            {
                return Content("ERROR");
            }
            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();

            var pre_id = WeixinPay.WeixinPayApi.Unifiedorder(order.ProductName, order.OrderId, order.TotalMoney, Request.UserHostAddress, openId);
            if (pre_id == "ERROR" || pre_id == "FAIL")
                return Content("ERROR");
            var package = "prepay_id=" + pre_id;

            var req = new RequestHandler(null);
            req.SetParameter("appId", AppId);
            req.SetParameter("timeStamp", timeStamp);
            req.SetParameter("nonceStr", nonceStr);
            req.SetParameter("package", package);
            req.SetParameter("signType", "MD5");
            var paySign = req.CreateMd5Sign("key", PayKey);

            //绑定页面数据
            ViewBag.TimeStamp = timeStamp;
            ViewBag.NonceStr = nonceStr;
            ViewBag.PaySign = paySign;
            ViewBag.Package = package;
            ViewBag.AppId = AppId;
            ViewBag.OrderId = order.OrderId;
            return View();
        }

        /// <summary>
        /// 微信回调
        /// </summary>
        /// <returns></returns>
        public ContentResult WexinPayNotify()
        {
            
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;
            //TODO:这里需要验证签名

            ////验证请求是否从微信发过来（安全）
            
            logger.Info("微信回调"+resHandler.ParseXML());
            //if (resHandler.IsTenpaySign())
            //{
            //    res = "success";
            //    logger.Info(resHandler.ParseXML());
            //    //正确的订单处理
                
            //}
            //else
            //{
            //    res = "wrong";

            //    //错误的订单处理
            //}
            if (return_code.ToLower() == "SUCCESS".ToLower())
            {
                OrderService.UpdateOrderPay(new OrderPayModel() { PayId = resHandler.GetParameter("prepay_id"), Status = 1 });
            }

            res = "success";
            //订单处理

            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>",
                return_code, return_msg);
            logger.Info("微信返回值"+xml);
            return Content(xml, "text/xml");
        }

        private readonly static string COOKIE_TOKEN_KEY = "wx_accessToken";
        private readonly static string COOKIE_REFRESHTOKEN_KEY = "wx_refreshToken";
        private readonly static string COOKIE_OPENID_KEY = "wx_openId";

        /// <summary>
        /// 网页请求授权
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns>openID，ERROR:错误</returns>
        public ContentResult WexinOpenOAuthCallBack(string code,string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;
            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(AppId, AppSecret, code);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Content("ERROR");
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                logger.Error(string.Format("调取WexinOpenOAuthCallBack出错,errorCode:{0},errormsg:{1}",result.errcode,result.errmsg));
                return Content("ERROR");
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Response.Cookies.Add(new HttpCookie(COOKIE_TOKEN_KEY, result.access_token));
            Response.Cookies.Add(new HttpCookie(COOKIE_REFRESHTOKEN_KEY, result.refresh_token));
            Response.Cookies.Add(new HttpCookie(COOKIE_OPENID_KEY, result.openid));

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            //try
            //{
            //    OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
            //    return View(userInfo);
            //}
            //catch (ErrorJsonResultException ex)
            //{
            //    return Content(ex.Message);
            //}
            return Content(result.openid);



        }
      
    }
}