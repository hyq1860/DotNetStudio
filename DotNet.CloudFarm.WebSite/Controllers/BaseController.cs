using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
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
    }
}