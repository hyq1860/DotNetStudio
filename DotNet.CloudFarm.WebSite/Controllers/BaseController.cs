using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.WebSite.MVC.Extensions;
using Microsoft.AspNet.Identity;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// controller基类
    /// </summary>
    public class BaseController:Controller
    {
        public BaseController()
        {
            
        }

        public BaseController(IUserService userService)
        {
            this.UserService = userService;
        }

        private IUserService UserService { get; set; }

        public UserModel UserInfo
        {
            get
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    var userId = int.Parse(this.User.Identity.GetUserId());
                    return UserService.GetUserByUserId(userId);
                }
                return null;
            }
        }

        /// <summary>
        /// 返回JsonResult.24         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult CustomJson(object data, JsonRequestBehavior behavior, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
        }

        /// <summary>
        /// 返回JsonResult42         /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <returns>Json</returns>
        protected JsonResult CustomJson(object data, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                FormateStr = format
            };
        }
    }
}