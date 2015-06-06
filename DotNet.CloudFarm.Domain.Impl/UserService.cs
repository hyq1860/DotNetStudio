using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl
{
    public class UserService:IUserService
    {
        public Result<LoginUser> Login(LoginUser loginUser)
        {
            throw new NotImplementedException();
        }

        public Result<LoginUser> GetCaptcha(string mobile)
        {
            throw new NotImplementedException();
        }
    }
}
