using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.Message;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Impl.Order;
using DotNet.CloudFarm.Domain.Model.Base;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.CloudFarm.WebSite.WeixinPay;
using DotNet.WebSite.Infrastructure.Config;
using DotNet.WebSite.MVC;
using log4net;
using Microsoft.AspNet.Identity;
using Senparc.Weixin.MP.TenPayLibV3;
using System.Net;
using System.IO;
using System.Net.Http;
using DotNet.CloudFarm.Domain.Contract.Address;
using DotNet.CloudFarm.Domain.Impl.Weather;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.CloudFarm.WebSite.Attributes;
using DotNet.Common.Collections;
using DotNet.Common.Models;
using DotNet.Common.Utility;
using Ninject;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using DotNet.CloudFarm.Domain.Contract.SMS;
using System.Text.RegularExpressions;
using DotNet.Common;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    [WebSiteAuthorize]
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService):base(userService)
        {
            
        }

        [Inject]
        public IAddressService AddressService { get; set; }

        [Inject]
        public IPreSaleOrderService PreSaleOrderService { get; set; }

        [Inject]
        public IPreSaleProductService PreSaleProductService { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IMessageService MessageService { get; set; }

        [Inject]
        public IOrderService OrderService { get; set; }

        [Inject]
        public ISMSService SMSService { get; set; }

        public ActionResult Default()
        {
            //var test=PreSaleProductService.GetPreSaleProducts();
            //var data=AddressService.GetAddresses();

            var homeViewModel = new HomeViewModel
            {
                Products = ProductService.GetProducts(1, 100, 1),//.Where(s=>s.CanSale).ToList(),
                //订单状态为已支付和待结算的羊的数量
                SheepCount = OrderService.GetProductCountWithStatus(this.UserInfo.UserId, new List<int>() { 1, 2 }),
                IsLogin = this.UserInfo != null && this.UserInfo.UserId > 0
            };
            return View(homeViewModel);
        }

        public ActionResult Index()
        {
            //数据库
            var user=UserService.GetUserByUserId(1);
            //var userId = UserService.Insert(new UserModel(){Mobile="13716457768",WxSex = 1,CreateTime = DateTime.Now});

            //读取配置文件 配置文件在网站Configs文件夹下的Params.config
            var test=ConfigHelper.ParamsConfig.GetParamValue("test");

            var userid=this.User.Identity.GetUserId();

            var result = UserService.Login(new LoginUser() {Mobile = "13716457768", Captcha = "123456"});
            return View();
        }

        public JsonResult Data()
        {
            //var result = new JsonResult();
            var homeViewModel = new HomeViewModel
            {
                Products = ProductService.GetProducts(1, 100, 1), 
                //订单状态为已支付和待结算的羊的数量
                SheepCount = OrderService.GetProductCountWithStatus(this.UserInfo.UserId,new List<int>(){1,2}),
                IsLogin=this.UserInfo!=null && this.UserInfo.UserId>0
            };
            //result.Data = homeViewModel;
            return this.CustomJson(homeViewModel, "yyyy-MM-dd HH:mm:ss");
            //return result;
        }

        public JsonResult GetProductById(int productId)
        {
            var result = new JsonResult();

            if (productId > 0)
            {
                result.Data = ProductService.GetProductById(productId);
            }
            
            return result;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Product()
        {
            return View();
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Order(int? productId)
        {
            try
            {
                var confirmOrderViewModel = new ConfirmOrderViewModel();
                if (productId.HasValue)
                {
                    confirmOrderViewModel.Product = ProductService.GetProductById(productId.Value);

                    confirmOrderViewModel.TopOrderInfos = OrderService.GetTopOrderList(1, 10);

                    var orderStatisModel = OrderService.GetOrderStatisModel(new List<int>(){10});
                    var info = orderStatisModel.UserOrderList.FirstOrDefault(s => s.UserId == this.UserInfo.UserId);
                    if (info != null)
                    {
                        var walletViewModel = OrderService.GetWalletViewModel(this.UserInfo.UserId, new List<int>() { 1, 2, 10 });
                        confirmOrderViewModel.Percentage = (((decimal)info.RowId / (decimal)orderStatisModel.TotalUserCount) * 100).ToString("F2");
                        confirmOrderViewModel.Earning = walletViewModel.TotalIncome;
                    }
                    else
                    {
                        confirmOrderViewModel.Earning = 0;
                        confirmOrderViewModel.Percentage = "0";
                    }
                }
                else
                {
                    return RedirectToAction("Default", "Home");
                }
                return View(confirmOrderViewModel);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
      
        }

        public JsonResult SubmitOrder(ConfirmOrderViewModel confirmOrderViewModel)
        {
            var orderModel = new OrderModel
            {
                OrderId = OrderService.GetNewOrderId(),
                UserId = this.UserInfo.UserId,
                ProductId = confirmOrderViewModel.Product.Id,
                Price = confirmOrderViewModel.Product.Price,
                ProductCount = confirmOrderViewModel.Count,
                Status = OrderStatus.Init.GetHashCode(),
                PayType = 0,
                CreateTime = DateTime.Now
            };
            var data=OrderService.SubmitOrder(orderModel);
            var result = new JsonResult();
            result.Data = data;
            return result;
        }

        #region

        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];

        /// <summary>
        /// 与微信公众账号后台的AppSecret设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        /// <summary>
        /// 微信支付KEY
        /// </summary>
        public static readonly string PayKey = WebConfigurationManager.AppSettings["WeixinPaySecretKey"];

        private ILog logger = LogManager.GetLogger("HomeController");

        /// <summary>
        /// 支付页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Pay(long? orderId,string act="pay")
        {
            try
            {
                var orderPayViewModel = new OrderPayViewModel();
                orderPayViewModel.Action = act;
                
                if (orderId.HasValue)
                {
                    var orderModel = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value, true);

                    if (orderModel != null)
                    {
                        var productModel = ProductService.GetProductById(orderModel.ProductId);

                        if (productModel != null)
                        {
                            orderPayViewModel.Name = productModel.Name;
                            orderPayViewModel.SheepType = productModel.SheepType;
                            orderPayViewModel.OrderId = orderId.Value;
                            orderPayViewModel.Price = productModel.Price;
                            orderPayViewModel.Count = orderModel.ProductCount;
                            orderPayViewModel.TotalPrice = productModel.Price * orderModel.ProductCount;
                            orderPayViewModel.StartTime = productModel.StartTime;
                            orderPayViewModel.EndTime = productModel.EndTime;
                            orderPayViewModel.ImgUrl = productModel.ImgUrl;
                            orderPayViewModel.PayType = orderModel.PayType;
                            if (act.ToLower() == "redeem")
                            {
                                orderPayViewModel.Action = "redeem";
                                orderPayViewModel.Principal = orderPayViewModel.TotalPrice;
                                orderPayViewModel.Earing = productModel.Earning*orderPayViewModel.Count;
                            }
                            else if (act.ToLower() == "pay")
                            {
                                if (!productModel.CanSale)
                                {
                                    return RedirectToAction("OrderList", "Home", new { tab = 3 });
                                }
                                orderPayViewModel.Action = "pay";
                                #region 微信支付



                                var userid = this.UserInfo.UserId;
                                var openId = this.UserInfo.WxOpenId;
                                var order = OrderService.GetOrderViewModel(userid, orderId.Value);
                                if (string.IsNullOrEmpty(order.ProductName) || order.OrderId == 0 || order.TotalMoney == 0M)
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
                                ViewBag.Uid =
                                    DotNet.Common.CryptographyHelper.Base64Encrypt(this.UserInfo.UserId.ToString());
                                OrderService.InsertOrderPay(new OrderPayModel()
                                {
                                    PayId = pre_id,
                                    OrdeId = order.OrderId,
                                    UserId = order.UserId,
                                    Status = 0,
                                    CreateTime = DateTime.Now
                                });

                                #endregion
                            }
                        }

                        
                    }
                }
                return View(orderPayViewModel);

            
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
           

        }

        /// <summary>
        /// 微信回调
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ContentResult WexinPayNotify()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");
            resHandler.SetKey(PayKey);
            string res = null;
            //TODO:这里需要验证签名

            ////验证请求是否从微信发过来（安全）
            //logger.Info("IsTenpaySign:" + resHandler.IsTenpaySign());
            if (resHandler.IsTenpaySign())
            {
                try
                {
                    //订单处理
                    if (return_code.ToLower() == "SUCCESS".ToLower())
                    {
                        string out_trade_no = resHandler.GetParameter("out_trade_no");
                        long orderId=0;
                        if (!string.IsNullOrEmpty(out_trade_no))
                        {
                            orderId = Convert.ToInt64(out_trade_no);
                        }
                        logger.Info("orderId:" + orderId+"|out_trade_no="+out_trade_no);
                        //判断老订单和预售订单
                        var flag = OrderService.CheckOrderExist(orderId);
                        if (flag)
                        {
                            OrderService.UpdateOrderPay(new OrderPayModel()
                            {
                                OrdeId = orderId,
                                //long.Parse(resHandler.GetParameter("out_trade_no")), 
                                Status = OrderStatus.Paid.GetHashCode()
                            });
                        }
                        else
                        {
                            PreSaleOrderService.ModifyPreOrder(new PreSaleOrder() {OrderId = orderId,Status = 1});
                            var preOrder = PreSaleOrderService.GetPreSaleOrder(orderId);
                            var mobile = preOrder.Phone;
                            SMSService.SendSMSPreOrderCreated(mobile, "12月19日");
                        }
                    }

                    res = "SUCCESS";

                }
                catch (Exception e)
                {
                    logger.Error("微信支付回调错误：" + e);
                    res = "FAIL";
                }
                string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>",
                res, return_msg);
                return Content(xml, "text/xml");
            }
            else
            {
                return Content("");
            }
        }

        public ActionResult PaySuccess(long orderId)
        {
            var payTipViewModel = new PayTipViewModel();
            var orderViewModel = OrderService.GetOrderViewModel(this.UserInfo.UserId, orderId);
            payTipViewModel.OrderId = orderViewModel.OrderId;
            payTipViewModel.PayMoney = orderViewModel.ProductCount*orderViewModel.Price;
            payTipViewModel.IsPaySuccess = orderViewModel.Status == OrderStatus.Paid.GetHashCode();
            payTipViewModel.Message = payTipViewModel.IsPaySuccess ? "支付成功" : "支付失败";
            payTipViewModel.BuyCount = orderViewModel.ProductCount;
            //获取时间戳
            var timestamp = JSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = JSSDKHelper.GetNoncestr();
            string ticket = JsApiTicketContainer.TryGetTicket(AppId, AppSecret);
            JSSDKHelper jsHelper = new JSSDKHelper();
            //获取签名
            var signature = jsHelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri);

            ViewData["AppId"] = AppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;

            ViewBag.uid = DotNet.Common.CryptographyHelper.Base64Encrypt(this.UserInfo.UserId.ToString());
            return View(payTipViewModel);
        }

        #endregion

        /// <summary>
        /// 用户中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCenter()
        {
            var hasUnRead = MessageService.CheckUnreadMessage(UserInfo.UserId);
            var myCenterViewModel = new MyCenterViewModel {User = UserInfo, IsHasNoReadMessage = hasUnRead};
            return View(myCenterViewModel);
        }

        public ActionResult OrderList(int pageIndex=1,int pageSize=10)
        {
            var result = OrderService.GetOrderList(this.UserInfo.UserId, pageIndex, pageSize);
            return View(result);
        }

        

        public JsonResult GetOrderList(int pageIndex = 1, int pageSize = 10)
        {
            var result = OrderService.GetOrderList(this.UserInfo.UserId, pageIndex, pageSize);
            return Json(result);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult CancelOrder(long? orderId)
        {
            var result = new JsonResult
            {
                Data = OrderService.UpdateOrderStatus(this.UserInfo.UserId, orderId.Value, OrderStatus.Close.GetHashCode())
            };
            return result;
        }
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult RedeemOrder(long? orderId)
        {
            var result = new JsonResult();
            if (orderId.HasValue)
            {
                var order = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value, true);

                if (order.Status != OrderStatus.Paid.GetHashCode())
                {
                    var data = new Result<OrderViewModel>
                    {
                        Status = new Status() {Code = "-1", Message = "当前订单状态不允许结算。"}
                    };
                    result.Data = data;
                    return result;
                }

                result.Data = OrderService.UpdateOrderStatus(this.UserInfo.UserId, orderId.Value,
                    OrderStatus.WaitingConfirm.GetHashCode());
            }
            return result;
        }

        /// <summary>
        /// 确认结算
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult ConfirmRedeemOrder(long? orderId,int payType)
        {
            var result = new JsonResult();
            if (orderId.HasValue)
            {
                var order = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value, true);

                if (order.Status != OrderStatus.Paid.GetHashCode())
                {
                    var data = new Result<OrderViewModel>
                    {
                        Status = new Status() { Code = "-1", Message = "当前订单状态不允许结算。" }
                    };
                    result.Data = data;
                    return result;
                }

                OrderService.UpdateOrderPayType(orderId.Value, this.UserInfo.UserId, payType);

                result.Data = OrderService.UpdateOrderStatus(this.UserInfo.UserId, orderId.Value,
                    OrderStatus.WaitingConfirm.GetHashCode());
            }
            return result;
        }


        public ActionResult MessageList(int pageIndex=1,int pageSize=10)
        {
            try
            {
                //ControllerContext.RequestContext.HttpContext
                var result = MessageService.GetMessages(this.UserInfo.UserId, pageIndex, pageSize);

                //修改短信已读状态
                MessageService.UpdateMessageStatus(this.UserInfo.UserId);
                return View(result);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View(new DotNet.Common.Models.Result<DotNet.CloudFarm.Domain.Model.Message.MessageModel>());
            }
        
        }

        public JsonResult GetMessageList(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                //ControllerContext.RequestContext.HttpContext
                var result = MessageService.GetMessages(this.UserInfo.UserId, pageIndex, pageSize);

                //修改短信已读状态
                MessageService.UpdateMessageStatus(this.UserInfo.UserId);
                return Json(result);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return Json(new DotNet.Common.Models.Result<PagedList<MessageModel>>());
            }

        }

        public ActionResult OrderDetail(long? orderId)
        {
            return View();
        }

        public ActionResult Contract()
        {
            return View();
        }

        /// <summary>
        /// 我的钱包
        /// </summary>
        /// <returns></returns>
        public ActionResult Wallet()
        {
            try
            {
                var walletViewModel = OrderService.GetWalletViewModel(this.UserInfo.UserId, new List<int>() { 1, 2, 10 });
                return View(walletViewModel);
            }
            catch (Exception e)
            {

                logger.Error(e);
                return View(new WalletViewModel());
            }

        }

        public JsonResult UpdateOrderPayType(long? orderId,int payType)
        {
            if (orderId.HasValue)
            {
                if (OrderService.UpdateOrderPayType(orderId.Value, this.UserInfo.UserId, payType))
                {
                    return
                    Json(new Result<object>() { Data = null, Status = new Status() { Code = "1", Message = "" } });
                }
                
            }

            return Json(new Result<object>() { Data = null, Status = new Status() { Code = "0", Message = "订单支付方式修改失败。" } });
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <returns></returns>
        public ActionResult TransferAccount(long orderId)
        {

            var payTipViewModel = new PayTipViewModel();
            var orderViewModel = OrderService.GetOrderViewModel(this.UserInfo.UserId, orderId);
            payTipViewModel.OrderId = orderViewModel.OrderId;
            payTipViewModel.PayMoney = orderViewModel.ProductCount * orderViewModel.Price;
            payTipViewModel.IsPaySuccess = orderViewModel.Status == OrderStatus.Paid.GetHashCode();
            payTipViewModel.Message = payTipViewModel.IsPaySuccess ? "支付成功" : "支付失败";
            payTipViewModel.BuyCount = orderViewModel.ProductCount;

            //获取时间戳
            var timestamp = JSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = JSSDKHelper.GetNoncestr();
            string ticket = JsApiTicketContainer.TryGetTicket(AppId, AppSecret);
            JSSDKHelper jsHelper = new JSSDKHelper();
            //获取签名
            var signature = jsHelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri);

            ViewData["AppId"] = AppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;

            ViewBag.uid = DotNet.Common.CryptographyHelper.Base64Encrypt(this.UserInfo.UserId.ToString());
            return View(payTipViewModel);
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public ActionResult Redeem()
        {
            return View();
        }

        /// <summary>
        /// 实时牧场
        /// </summary>
        /// <returns></returns>
        public ActionResult Video()
        {
            return View();
        }

        public ContentResult GetWeather()
        {
            return Content(GetWeatherByHttp());
        }

        private string GetWeatherByHttp()
        {
            //WebRequest request = WebRequest.Create("http://www.weather.com.cn/adat/cityinfo/101080701.html");
            //request.Method = "GET";
            //HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
            // string responseContent ="";
            //using(StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            //{
            //    responseContent = streamReader.ReadToEnd();
            //}
            //return responseContent;
            //HttpClient client=new HttpClient();
            //var task= client.GetStringAsync("http://www.weather.com.cn/adat/cityinfo/101080701.html");
            //return task.Result;

            return WeatherAPI.InStance.WeatherData.Data;
        }

        [AllowAnonymous]
        public ActionResult Share(string uid)
        {
            string strUserId = DotNet.Common.CryptographyHelper.Base64Decrypt(uid);
            if (strUserId == uid)
            {

            }
            else
            {
                var userInfo = UserService.GetUserByUserId(int.Parse(strUserId));
                if (userInfo != null)
                {
                    var orderStatisModel = OrderService.GetOrderStatisModel(new List<int>(){1,2,10});
                    var info = orderStatisModel.UserOrderList.FirstOrDefault(s => s.UserId == userInfo.UserId);
                    if (info != null)
                    {
                        var walletViewModel = OrderService.GetWalletViewModel(userInfo.UserId, new List<int>() { 1, 2, 10 });
                        ViewBag.HeadUrl = userInfo.WxHeadUrl;
                        ViewBag.UserName = userInfo.WxNickName;
                        var rankingCssClass = "";
                        if (info.RowId == 1)
                        {
                            rankingCssClass = "gold";
                        }
                        else if (info.RowId == 2)
                        {
                            rankingCssClass = "silver";
                        }
                        else
                        {
                            rankingCssClass = "copper";
                        }

                        ViewBag.RankingCssClass = rankingCssClass;
                        ViewBag.SheepCount = OrderService.GetProductCountWithStatus(userInfo.UserId,
                            new List<int>() {1, 2});
                        ViewBag.Percentage = (((decimal)info.RowId / (decimal)orderStatisModel.TotalUserCount) * 100);
                        ViewBag.YearEarningRate = walletViewModel.YearEarningRate;
                    }
                    else
                    {
                        ViewBag.Earning = 0;
                        ViewBag.YearEarningRate = 0;
                        ViewBag.SheepCount = 0;
                        ViewBag.Percentage = 0;
                    }
                }
            }
            
            return View();
        }


        public ActionResult Active()
        {
            var condition = "price=1 and StartTime<=GETDATE() and EndTime>=GETDATE() and [status]=0";
            var product = ProductService.GetProductByCondition(condition);
            if(product.Id>0)
            {
                return RedirectToAction("Order", new { productId = product.Id });
            }
            return Content("活动已经结束，敬请期待下期");

        }

        #region 分享统一接口

        [AllowAnonymous]
        public JsonResult GetShareParms()
        {
            //获取时间戳
            var timestamp = JSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = JSSDKHelper.GetNoncestr();
            string ticket = JsApiTicketContainer.TryGetTicket(AppId, AppSecret);
            JSSDKHelper jsHelper = new JSSDKHelper();
            //获取签名
            var signature = jsHelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri);
            var obj = new {
                appid = AppId,
                timestamp = timestamp,
                nonceStr = nonceStr,
                signature = signature
            };

            return Json(obj);
        }

        #endregion

        #region 预售
        [AllowAnonymous]
        public ActionResult PreSaleProduct()
        {
            var products=PreSaleProductService.GetPreSaleProducts(p=>p.IsSale,p=>p.CreateTime, "order");

            #region 分享相关
            //获取时间戳
            var timestamp = JSSDKHelper.GetTimestamp();
            //获取随机码
            var nonceStr = JSSDKHelper.GetNoncestr();
            string ticket = JsApiTicketContainer.TryGetTicket(AppId, AppSecret);
            JSSDKHelper jsHelper = new JSSDKHelper();
            //获取签名
            var signature = jsHelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri);

            ViewData["AppId"] = AppId;
            ViewData["Timestamp"] = timestamp;
            ViewData["NonceStr"] = nonceStr;
            ViewData["Signature"] = signature;


            #endregion
            return View(products);
        }

        public ActionResult ConfirmPreSaleOrder(int productId)
        {
            PreSaleOrderViewModel preSaleOrderViewModel=new PreSaleOrderViewModel();

            
            preSaleOrderViewModel.OrderId = OrderService.GetNewOrderId();
            //预售商品
            preSaleOrderViewModel.PreSaleProduct=PreSaleProductService.GetPreSaleProduct(productId);

            preSaleOrderViewModel.PreSaleProduct.Details =
                JsonHelper.FromJson<List<PreSaleProductDetail>>(preSaleOrderViewModel.PreSaleProduct.DetailJson);
            foreach (var detail in preSaleOrderViewModel.PreSaleProduct.Details)
            {
                preSaleOrderViewModel.DetailsStr += string.Format("{0} {1}*{2}|", detail.Name, detail.Weight,
                    detail.Count);
            }
            if (!string.IsNullOrEmpty(preSaleOrderViewModel.DetailsStr))
            preSaleOrderViewModel.DetailsStr =
                preSaleOrderViewModel.DetailsStr.Remove(preSaleOrderViewModel.DetailsStr.Length - 1, 1);

            //加载地址及上一次订单地址
            var addresses = AddressService.GetAddresses();
            if (preSaleOrderViewModel.PreSaleProduct.BeiJinLimit == 1)
            {
                preSaleOrderViewModel.Provinces = addresses.Where(s => s.ParentCode == "" && (s.Code == "110000" || s.Code == "120000" || s.Code == "130000")).ToList();
            }
            else
            {
                preSaleOrderViewModel.Provinces = addresses.Where(s => s.ParentCode == "").ToList();
            }

            ////去除上一次的地址
            //var lastOrder = PreSaleOrderService.GetPreSaleOrderList(o=>o.UserId ==this.UserInfo.UserId,1, 1).Data.FirstOrDefault();
            //if (lastOrder != null)
            //{
            //    if (preSaleOrderViewModel.PreSaleProduct.BeiJinLimit == 1&& lastOrder.ProvinceId!= "110000")
            //    {
            //        preSaleOrderViewModel.ProvinceId = "110000";
            //    }
            //    else
            //    {
            //        preSaleOrderViewModel.ProvinceId = lastOrder.ProvinceId;
            //        preSaleOrderViewModel.CityId = lastOrder.CityId;
            //        preSaleOrderViewModel.AreaId = lastOrder.Code;
            //        preSaleOrderViewModel.Address = lastOrder.Address;
            //        preSaleOrderViewModel.UserName = lastOrder.Receiver;
            //        preSaleOrderViewModel.Phone = lastOrder.Phone;
            //    }

            //}
            ////OrderService.GetPreSaleOrderList(1, 1, 1);
            

            if (!string.IsNullOrEmpty(preSaleOrderViewModel.ProvinceId))
            {
                preSaleOrderViewModel.Cities =
                addresses.Where(s => s.ParentCode == preSaleOrderViewModel.ProvinceId).ToList();
                if (!string.IsNullOrEmpty(preSaleOrderViewModel.AreaId))
                {
                    preSaleOrderViewModel.Areas =
                        addresses.Where(s => s.ParentCode == preSaleOrderViewModel.CityId).ToList();
                }
            }
            
         
            return View(preSaleOrderViewModel);
        }

        public ActionResult SubmitPreSaleOrder(PreSaleOrderViewModel preSaleOrderViewModel)
        {
            var jsonResult=new JsonResult();
            if (!string.IsNullOrEmpty(preSaleOrderViewModel.InviteCode))
            {
                preSaleOrderViewModel.InviteCode = preSaleOrderViewModel.InviteCode.Replace(" ", "");
            }
            var preSaleOrder = new PreSaleOrder
            {
                OrderId = preSaleOrderViewModel.OrderId,//OrderService.GetNewOrderId(),
                UserId = this.UserInfo.UserId,
                ProductId = preSaleOrderViewModel.PreSaleProduct.ProductId,
                Price = preSaleOrderViewModel.PreSaleProduct.Price,
                Count = preSaleOrderViewModel.Count,
                ProvinceId = preSaleOrderViewModel.ProvinceId,
                CityId = preSaleOrderViewModel.CityId,
                Code = preSaleOrderViewModel.AreaId,
                Address = preSaleOrderViewModel.Address,
                Status = 0,
                CreateTime = DateTime.Now,
                Receiver = preSaleOrderViewModel.UserName,
                Phone=preSaleOrderViewModel.Phone,
                TotalMoney = preSaleOrderViewModel.Count* preSaleOrderViewModel.PreSaleProduct.Price,
                ExpressDelivery = string.Empty,
                InviteCode = preSaleOrderViewModel.InviteCode
            };

            

            var result=PreSaleOrderService.SubmitPreSaleOrder(preSaleOrder);

            if (result > 0)
            {
                #region 微信支付所需数据计算
                var timeStamp = TenPayV3Util.GetTimestamp();
                var nonceStr = TenPayV3Util.GetNoncestr();
                var openId = this.UserInfo.WxOpenId;
                var pre_id = WeixinPay.WeixinPayApi.Unifiedorder(preSaleOrderViewModel.PreSaleProduct.Name,
                    preSaleOrder.OrderId, preSaleOrder.Price* preSaleOrder.Count, Request.UserHostAddress, openId);
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


                #endregion

                preSaleOrderViewModel.NonceStr = nonceStr;
                preSaleOrderViewModel.TimeStamp = timeStamp;
                preSaleOrderViewModel.Package = package;
                preSaleOrderViewModel.AppId = AppId;
                preSaleOrderViewModel.PaySign = paySign;
            }
            else
            {
                preSaleOrderViewModel.OrderId = 0;
            }

            jsonResult.Data = preSaleOrderViewModel;
            return jsonResult;
        }

        public ActionResult GetCities(string provinceCode)
        {
            
            var addresses = AddressService.GetChildrenByParentId(provinceCode);
            
            return Content(JsonHelper.ToJson(addresses));
        }

        public ActionResult GetAreas(string cityCode)
        {
            //var jsonResult = new JsonResult();
            var addresses = AddressService.GetChildrenByParentId(cityCode);
            //jsonResult.Data = addresses;
            return Content(JsonHelper.ToJson(addresses));
        }

        /// <summary>
        /// 预售订单列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult PreSaleOrderList(int pageIndex = 1, int pageSize = 10)
        {
            //前台订单列表只显示支付成功的订单
            var result=PreSaleOrderService.GetPreSaleOrderList(p => p.UserId == this.UserInfo.UserId&&(p.Status==1||p.Status==2), pageIndex, pageSize);
            return View(result);
        }

        /// <summary>
        /// 页面访问记录
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult PageVisit(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return Json(false);
            }
            var ip = GetIpaddress();
            var result   = UserService.InsertPageLog(new PageLog() {
                Ip=ip, PageSource=source
            });
            return Json(result);



        }

        /// <summary>
        ///   获取IP地址
        /// </summary>
        /// <returns></returns>
        public  string GetIpaddress()
        {
            string result = String.Empty;
            result = HttpContext.Request.ServerVariables["HTTP_CDN_SRC_IP"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !IsIP(result))
                return "127.0.0.1";

            return result;
        }
        /// <summary>
        /// 判断是否为IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public  bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }
        #endregion


        #region 二维码

        public ContentResult GetQRCode(string openId)
        {
            var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);
            var qrResult = Senparc.Weixin.MP.AdvancedAPIs.QrCode.QrCodeApi.Create(accesstoken, 1800, UserInfo.UserId);
            var link = Senparc.Weixin.MP.AdvancedAPIs.QrCode.QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            Response.Redirect(link, true);
            return Content("");
        }
        #endregion


        #region 赠送

        /// <summary>
        /// 赠送页面
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult SendGift(long? orderId)
        {
            var order=new OrderModel();
            if (orderId.HasValue)
            {
                order=OrderService.GetOrder(this.UserInfo.UserId, orderId.Value, true);
                ViewBag.SelfMobile = this.UserInfo.Mobile;
            }
            return View(order);
        }

        /// <summary>
        /// 赠送ajax处理
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public ActionResult ProcessSendGift(long orderId,string mobile,string remark)
        {
            var jsonResult = new JsonResult();
            var sendUser = UserService.GetUser(mobile);
            var result = new Result<OrderModel>();
            if (sendUser==null)
            {
                result.Status = new Status() { Code="0",Message= "被赠送人尚未登录过羊客，不能被赠送。" };
            }
            else
            {
                result=OrderService.SendGift(orderId, this.UserInfo.UserId, sendUser.UserId,remark);
            }
            jsonResult.Data = result;
            return jsonResult;
        }

        public ActionResult GetSendUserName(string mobile)
        {
            var jsonResult = new JsonResult();
            var sendUser = UserService.GetUser(mobile);
            var result = new Result<UserModel>();
            if (sendUser == null||sendUser.UserId<=0)
            {
                result.Status = new Status() { Code = "0", Message = "被赠送人尚未登录过羊客，不能被赠送。" };
                result.Data = sendUser;
            }
            else
            {
                result.Status = new Status() { Code = "1" };
                result.Data = sendUser;
            }
            jsonResult.Data = result;
            return jsonResult;
        }

        public ActionResult ShareGift(long? orderId)
        {
            var order = new OrderModel();
            if (orderId.HasValue)
            {
                order = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value,false);
                if (order != null)
                {
                    ViewBag.UserInfo = this.UserInfo;
                    ViewBag.SendUserInfo = UserService.GetUserByUserId(order.SendUserId);
                    ViewBag.Order = order;
                }
            }

            return View();
        }

        /// <summary>
        /// 送出的订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GiftList()
        {
            var userIds=new List<int>();
            //送出的订单列表
            var sendGiftList=OrderService.GetSendOrderList(this.UserInfo.UserId, 1, 100);
            //收到的订单列表
            var receiveGiftList=OrderService.GetReceiveOrderList(this.UserInfo.UserId, 1, 100);
            userIds.AddRange(sendGiftList.Data.Select(s => s.UserId).ToList());
            userIds.AddRange(receiveGiftList.Data.Select(s => s.SendUserId).ToList());
            var users = UserService.GetUsers(userIds);

            foreach (var orderViewModel in sendGiftList.Data)
            {
                var user = users.FirstOrDefault(s => s.UserId == orderViewModel.UserId);
                if (user != null)
                {
                    orderViewModel.SendUserName = user.WxNickName;
                    orderViewModel.SendUserMobile = user.Mobile.HideMiddle(3, 4);
                }
            }

            foreach (var orderViewModel in receiveGiftList.Data)
            {
                var user = users.FirstOrDefault(s => s.UserId == orderViewModel.SendUserId);
                if (user != null)
                {
                    orderViewModel.SendUserName = user.WxNickName;
                    orderViewModel.SendUserMobile = user.Mobile.HideMiddle(3, 4);
                }
            }
            ViewBag.SendGiftList = sendGiftList.Data;
            ViewBag.ReceiveGiftList = receiveGiftList.Data;
            return View();
        }

        /// <summary>
        /// 赠送订单成功
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public ActionResult SendGiftSuccess(long? orderId)
        {
            var order = new OrderViewModel();
            if (orderId.HasValue)
            {
                var tempOrder = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value,false);
                if (tempOrder != null)
                {
                    var user = UserService.GetUserByUserId(tempOrder.UserId);
                    if (user != null)
                    {
                        order.SendUserMobile = user.Mobile;
                        order.SendUserName = user.WxNickName;
                        order.OrderId = tempOrder.OrderId;

                        //获取时间戳
                        var timestamp = JSSDKHelper.GetTimestamp();
                        //获取随机码
                        var nonceStr = JSSDKHelper.GetNoncestr();
                        string ticket = JsApiTicketContainer.TryGetTicket(AppId, AppSecret);
                        JSSDKHelper jsHelper = new JSSDKHelper();
                        //获取签名
                        var signature = jsHelper.GetSignature(ticket, nonceStr, timestamp, Request.Url.AbsoluteUri);

                        ViewData["AppId"] = AppId;
                        ViewData["Timestamp"] = timestamp;
                        ViewData["NonceStr"] = nonceStr;
                        ViewData["Signature"] = signature;
                    }
                }
            }

            return View(order);
        }

        #endregion
    }
}