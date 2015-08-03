using DotNet.CloudFarm.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Impl.User;
using Microsoft.AspNet.Identity;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    public class BaseHouTaiController : Controller
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


        public BaseHouTaiController(IUserService userService)
        {
            this.UserService = userService;
        }
        /// <summary>
        /// 当前登录管理员
        /// </summary>
        public AdminUser Admin {

            get
            {
                var adminUser = new AdminUser();
                if (this.User.Identity.IsAuthenticated)
                {
                    var userId = int.Parse(this.User.Identity.GetUserId());
                    var user=UserService.FindBackstageLoginUserByUserId(userId);
                    if (user != null)
                    {
                        adminUser.Id = user.UserId;
                        adminUser.AdminName = user.UserName;
                    }
                    return adminUser;
                }
                return null;
            }
        }

        private IUserService UserService { get; set; }

        
    }
}