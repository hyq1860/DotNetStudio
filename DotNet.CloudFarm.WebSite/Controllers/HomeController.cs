﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.WebSite.Infrastructure.Config;
using DotNet.WebSite.MVC;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class HomeController : Controller
    {
        [NinjectService]
        public IUserService UserService { get; set; }

        public ActionResult Index()
        {
            //数据库
            //new UserDataAccess().GetUsers();

            //读取配置文件 配置文件在网站Configs文件夹下的Params.config
            var test=ConfigHelper.ParamsConfig.GetParamValue("test");

            var result = UserService.Login(new LoginUser() {Mobile = "13716457768", Captcha = "123456"});
            return View();
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
    }
}