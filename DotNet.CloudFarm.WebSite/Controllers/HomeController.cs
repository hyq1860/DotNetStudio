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

namespace DotNet.CloudFarm.WebSite.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService):base(userService)
        {
            
        }

        [Ninject.Inject]
        public IUserService UserService { get; set; }

        [Ninject.Inject]
        public IProductService ProductService { get; set; }

        [Ninject.Inject]
        public IMessageService MessageService { get; set; }

        [Ninject.Inject]
        public IOrderService OrderService { get; set; }

        public ActionResult Default()
        {
            return View();
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
                Products = ProductService.GetProducts(1, 5, 1), 
                SheepCount = OrderService.GetProductCountWithStatus(this.UserInfo.UserId,new List<int>(){1})
            };
            //result.Data = homeViewModel;
            return this.CustomJson(homeViewModel, "yyyy年MM月dd日");
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
            var confirmOrderViewModel = new ConfirmOrderViewModel();
            if (productId.HasValue)
            {
                confirmOrderViewModel.Product = ProductService.GetProductById(productId.Value);

                confirmOrderViewModel.TopOrderInfos = OrderService.GetTopOrderList(1, 3);
            }
            else
            {
                return RedirectToAction("Default", "Home");
            }
            return View(confirmOrderViewModel);
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
        public ActionResult Pay(long? orderId)
        {
            try
            {
                var orderPayViewModel = new OrderPayViewModel();
                if (orderId.HasValue)
                {
                    var orderModel = OrderService.GetOrder(this.UserInfo.UserId, orderId.Value);

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
                        }

                        #region 微信支付

                        //TODO:将该页加入登录页,就可以启用下边的注释
                        //var userid = UserInfo.UserId;
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
            logger.Info("wexinpayNotify");
            ResponseHandler resHandler = new ResponseHandler(null);

            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");

            string res = null;
            //TODO:这里需要验证签名

            ////验证请求是否从微信发过来（安全）

            logger.Info("微信回调" + resHandler.ParseXML());
            if (resHandler.IsTenpaySign())
            {
                try
                {
                    //订单处理
                    if (return_code.ToLower() == "SUCCESS".ToLower())
                    {
                        OrderService.UpdateOrderPay(new OrderPayModel() { OrdeId = 81150721544370,
                            //long.Parse(resHandler.GetParameter("out_trade_no")), 
                            Status = 1 });
                    }

                    res = "SUCCESS";

                }
                catch (Exception e)
                {
                    logger.Error("微信支付回调错误：" + e);
                    res = "FAIL";
                }
                string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>",
                return_code, return_msg);
                logger.Info("微信返回值" + xml);
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
            return View(payTipViewModel);
        }

        #endregion

        /// <summary>
        /// 用户中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCenter()
        {
            var myCenterViewModel = new MyCenterViewModel {User = UserInfo, IsHasNoReadMessage = true};
            return View(myCenterViewModel);
        }

        public ActionResult OrderList(int pageIndex=1,int pageSize=10)
        {
            var result = OrderService.GetOrderList(this.UserInfo.UserId, pageIndex, pageSize);
            return View(result);
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
                Data = OrderService.UpdateOrderStatus(this.UserInfo.UserId, orderId.Value, -1)
            };
            return result;
        }

        public ActionResult MessageList(int pageIndex=1,int pageSize=10)
        {
            //ControllerContext.RequestContext.HttpContext
            var result = MessageService.GetMessages(this.UserInfo.UserId, pageIndex, pageSize);

            //修改短信已读状态
            MessageService.UpdateMessageStatus(this.UserInfo.UserId);
            return View(result);
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
            var walletViewModel = OrderService.GetWalletViewModel(this.UserInfo.UserId,new List<int>(){1,2,10});
            return View(walletViewModel);
        }

        /// <summary>
        /// 转账
        /// </summary>
        /// <returns></returns>
        public ActionResult TransferAccount()
        {
            
            return View();
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public ActionResult Redeem()
        {
            return View();
        }
    }
}