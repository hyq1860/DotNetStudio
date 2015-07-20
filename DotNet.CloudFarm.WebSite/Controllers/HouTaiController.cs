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
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Contract.WeiXin;
using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using DotNet.CloudFarm.Domain.Model.WeiXin;
using DotNet.CloudFarm.Domain.Model.Order;
using DotNet.CloudFarm.Domain.Model.Product;
using DotNet.CloudFarm.Domain.Model.Base;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.CloudFarm.Domain.Contract.SMS;
using DotNet.CloudFarm.Domain.Impl.SMS;
using DotNet.CloudFarm.WebSite.WeixinPay;


namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// 后台
    /// </summary>
    public class HouTaiController : BaseHouTaiController
    {
        [Ninject.Inject]
        public IProductService ProductService { get; set; }
        /// <summary>
        /// 微信相关业务
        /// </summary>
        [Ninject.Inject]
        public IWeiXinService WeiXinService { get; set; }
     
        
        [Ninject.Inject]
        public IOrderService OrderService { get; set; }

        /// <summary>
        /// 用户服务
        /// </summary>
        [Ninject.Inject]
        public IUserService UserService { get; set; }

        /// <summary>
        /// 短信服务
        /// </summary>
        [Ninject.Inject]
        public ISMSService SMSService { get; set; }
        
        public ActionResult Index()
        {
            return View();
        }

        #region 微信菜单
        /// <summary>
        /// 微信底部菜单管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WeixinMenu()
        {
            var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);
            var menu = CommonApi.GetMenu(accesstoken);
            if(menu!=null)
            {
                ViewBag.MenuList = menu.menu;
            }
            else
            {
                ViewBag.MenuList = null;
            }
            return View();
        }

        #endregion

        #region 微信回复

        /// <summary>
        /// 微信回复内容设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WeixinMessage()
        {
            var accesstoken = AccessTokenContainer.TryGetToken(AppId, AppSecret);
            var autoReplyMessagelist = WeiXinService.AutoReplyMessageGetAll();
            ViewBag.MessageList = autoReplyMessagelist;
            return View();
        }
        /// <summary>
        /// 编辑或添加微信回复内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WeixinMessage(WeixinAutoReplyMessageModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreatorId = this.Admin.Id;
            if(model.Id==0)
            {
                WeiXinService.AutoReplyMessageInsert(model);
            }
            else
            {
                WeiXinService.AutoReplyMessageUpdate(model);
            }
            var autoReplyMessagelist = WeiXinService.AutoReplyMessageGetAll();
            ViewBag.MessageList = autoReplyMessagelist;
            return View();
        }
        [HttpPost]
        public JsonResult CheckKeyword(string keyword)
        {
            var result = WeiXinService.AutoReplyMessageCheckKeyword(keyword);
            return Json(result);
        }
        [HttpPost]
        public JsonResult DelKeyword(int id)
        {
            WeiXinService.AutoReplyMessageUpdateStatus(id, 0);
            return Json(1);
        }

        #endregion

        #region 产品后台

        /// <summary>
        /// 产品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Product(int pageIndex=1,int pageSize=10)
        {
            var status = 1;//调取显示的
            var productList =ProductService.GetProducts(pageIndex,pageSize,status);
            ViewBag.ProductList = productList;
            return View();
        }
        /// <summary>
        /// 新增或编辑product页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProductAddOrEdit(int id=0)
        {
            if(id>0)
            {
                ViewBag.Product = ProductService.GetProductById(id);
            }
            return View();
        }

        /// <summary>
        /// 添加或编辑
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductAddOrEdit(ProductModel product)
        {
            product.CreateTime = DateTime.Now;
            product.CreatorId = Admin.Id;

            if (Request.Files.Count > 0 && Request.Files[0].ContentLength>0)
            {
                var file = Request.Files[0];
                var urlPath = string.Format("/images/upload/{0}/", DateTime.Now.ToString("yyyyMMdd"));
                var imgFilePath = Server.MapPath(urlPath);
                var imgName = string.Format("{0}.jpg",Guid.NewGuid());
                if (!Directory.Exists(imgFilePath))
                {
                    Directory.CreateDirectory(imgFilePath);
                }
                file.SaveAs(Path.Combine(imgFilePath,imgName));
                product.ImgUrl = string.Format("{0}{1}",urlPath,imgName);
            }
            else
            {
                if(string.IsNullOrEmpty(product.ImgUrl))
                {
                    product.ImgUrl = "/images/no_pic.jpg";
                }
            }
            if(product.Id==0)
            {
                //insert
                ProductService.InsertProduct(product);
            }
            else if(product.Id>0)
            {
                //update
                ProductService.UpdateProduct(product);

            }
            return RedirectToAction("Product");
        }

        /// <summary>
        /// AJAX获取所有product
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetProducts(int pageSize=20,int pageIndex=1)
        {
            var productList = ProductService.GetProducts(pageIndex, pageSize);
            var result = new
            {
                PageIndex = productList.PageIndex,
                PageSize = productList.PageSize,
                Products = productList.ToList(),
                Count = productList.TotalCount,
                PageNo=productList.TotalCount % productList.PageSize != 0 ? (productList.TotalCount / productList.PageSize) + 1 : productList.TotalCount / productList.PageSize
            };
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AJAX删除product
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DelProduct(int id, int pageSize = 20, int pageIndex = 1)
        {
            var status = -1;
            ProductService.UpdateStatus(id,status);
            var productList = ProductService.GetProducts(pageIndex, pageSize);
            var result = new
            {
                PageIndex = productList.PageIndex,
                PageSize = productList.PageSize,
                Products = productList.ToList(),
                Count = productList.TotalCount,
                PageNo=productList.TotalCount % productList.PageSize != 0 ? (productList.TotalCount / productList.PageSize) + 1 : productList.TotalCount / productList.PageSize
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 订单后台
        public ActionResult OrderList()
        {
            return View();
        }

        public JsonResult GetOrderList(int pageSize=20, int pageIndex=1)
        {
            var orderList = OrderService.GetOrderList(pageIndex, pageSize);
            var result = new
            {
                PageIndex = orderList.Data.PageIndex,
                PageSize = orderList.Data.PageSize,
                List = orderList.Data.ToList(),
                Count = orderList.Data.TotalCount,
                PageNo = orderList.Data.TotalCount % orderList.Data.PageSize != 0 ?
                    (orderList.Data.TotalCount / orderList.Data.PageSize) + 1 : orderList.Data.TotalCount / orderList.Data.PageSize
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult CancelOrder(long orderId,int userId,int pageIndex,int pageSize)
        {
            var status = OrderStatus.Close.GetHashCode();
            var orderList = ChangeOrderStatus(orderId, userId, pageIndex, pageSize, status);
            var result = new
            {
                PageIndex = orderList.Data.PageIndex,
                PageSize = orderList.Data.PageSize,
                List = orderList.Data.ToList(),
                Count = orderList.Data.TotalCount,
                PageNo = orderList.Data.TotalCount % orderList.Data.PageSize != 0 ?
                    (orderList.Data.TotalCount / orderList.Data.PageSize) + 1 : orderList.Data.TotalCount / orderList.Data.PageSize
            };
            return Json(result,JsonRequestBehavior.AllowGet);
            
        }

        /// <summary>
        /// 确认支付
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult CofirmOrderPay(long orderId, int userId, int pageIndex, int pageSize)
        {
            var status = OrderStatus.Paid.GetHashCode();
            var orderList = ChangeOrderStatus(orderId, userId, pageIndex, pageSize, status);
            var result = new
            {
                PageIndex = orderList.Data.PageIndex,
                PageSize = orderList.Data.PageSize,
                List = orderList.Data.ToList(),
                Count = orderList.Data.TotalCount,
                PageNo = orderList.Data.TotalCount % orderList.Data.PageSize != 0 ?
                    (orderList.Data.TotalCount / orderList.Data.PageSize) + 1 : orderList.Data.TotalCount / orderList.Data.PageSize
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 确认赎回
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public JsonResult ConfirmOrderPayReturn(long orderId, int userId, int pageIndex, int pageSize)
        {
            var status = OrderStatus.Complete.GetHashCode();
            var order = OrderService.GetOrder(userId,orderId);
            var user = UserService.GetUserByUserId(userId);
            var product = ProductService.GetProductById(order.ProductId);
            if(order.Status==OrderStatus.WaitingConfirm.GetHashCode() && product.EndTime.AddDays(product.EarningDay)>DateTime.Now)
            {
                //TODO:调取微信企业支付接口
                var totalRefund = order.Price * order.ProductCount + product.Earning;
                var description = string.Format("羊客【{0}】回购",product.Name);
                var payResult =  WeixinPayApi.QYPay(user.WxOpenId, orderId, totalRefund, description);
                if(payResult=="ERROR")
                {
                    //错误处理
                }
            }
      
            var orderList = ChangeOrderStatus(orderId, userId, pageIndex, pageSize, status);
            var result = new
            {
                PageIndex = orderList.Data.PageIndex,
                PageSize = orderList.Data.PageSize,
                List = orderList.Data.ToList(),
                Count = orderList.Data.TotalCount,
                PageNo = orderList.Data.TotalCount % orderList.Data.PageSize != 0 ?
                    (orderList.Data.TotalCount / orderList.Data.PageSize) + 1 : orderList.Data.TotalCount / orderList.Data.PageSize
            };
                 
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 变更订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private Common.Models.Result<Common.Collections.PagedList<OrderManageViewModel>> ChangeOrderStatus(long orderId, int userId, int pageIndex, int pageSize, int status)
        {
            var orderResult = OrderService.UpdateOrderStatus(userId, orderId, status);
            var orderList = OrderService.GetOrderList(pageIndex, pageSize);
            return orderList;
        }
        #endregion

        #region 用户后台
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserList()
        {
            return View();
        }

        /// <summary>
        /// AJAX获取用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetUserList(int pageIndex=1,int pageSize=20)
        {
            var userList = UserService.GetUserList(pageIndex, pageSize);
            var result = new
            {
                PageIndex = userList.PageIndex,
                PageSize = userList.PageSize,
                List = userList.ToList(),
                Count = userList.TotalCount,
                PageNo = userList.TotalCount % userList.PageSize != 0 ?
                    (userList.TotalCount / userList.PageSize) + 1 :
                    userList.TotalCount / userList.PageSize
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult DisableUser(int userId)
        {
            var status = 0;
            var result = UserService.UpdateUserStatus(userId, status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JsonResult EnableUser(int userId)
        {
            var status = 1;
            var result = UserService.UpdateUserStatus(userId, status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public JsonResult SearchUser(string mobile)
        {
            var user = UserService.GetUser(mobile);
            var result = new
            {
                PageIndex = 1,
                PageSize = 20,
                List = new List<UserModel>(){user},
                Count = 1,
                PageNo = 1,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ContentResult TestSMS(string tel)
        {
            var value = SMSService.SendSMSOrderCreated(tel, 111111111, 10.00M);
            return Content(value.ToString());
        }
    }
}