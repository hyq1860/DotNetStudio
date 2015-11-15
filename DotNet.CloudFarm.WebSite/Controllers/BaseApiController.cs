using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Impl.SMS;
using DotNet.CloudFarm.Domain.Impl.User;
using DotNet.CloudFarm.Domain.Model;
using DotNet.CloudFarm.Domain.Model.User;
using Microsoft.AspNet.Identity;

namespace DotNet.CloudFarm.WebSite.Controllers
{
    /// <summary>
    /// ApiController基类
    /// </summary>
    public class BaseApiController:ApiController
    {
        private IUserService UserService=new UserService(new UserDataAccess(),new SMSService(),new CloudFarmDbContext());

        public UserModel UserInfo
        {
            get
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    return UserService.GetUserByUserId(int.Parse(this.User.Identity.GetUserId()));
                }
                return null;
            }
        }
    }
}