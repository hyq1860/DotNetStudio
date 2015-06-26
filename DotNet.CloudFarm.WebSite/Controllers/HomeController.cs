using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.CloudFarm.Domain.ViewModel;
using DotNet.WebSite.Infrastructure.Config;
using DotNet.WebSite.MVC;
using Microsoft.AspNet.Identity;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class HomeController : BaseController
    {
        [Ninject.Inject]
        public IUserService UserService { get; set; }

        [Ninject.Inject]
        public IProductService ProductService { get; set; }

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
            var result = new JsonResult();
            var homeViewModel = new HomeViewModel
            {
                Products = ProductService.GetProducts(1, 5, 1), 
                SheepCount = 188
            };
            result.Data = homeViewModel;
            return result;
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
            return View();
        }
    }
}