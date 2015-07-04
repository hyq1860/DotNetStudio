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

namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// 后台
    /// </summary>
    public class HouTaiController : BaseHouTaiController
    {
        
        public ActionResult Index()
        {
            return View();
        }


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


    }
}