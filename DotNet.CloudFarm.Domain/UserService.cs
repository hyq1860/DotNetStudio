using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract;

namespace DotNet.CloudFarm.Domain
{
    public class UserService : IUserService
    {
        public string Login()
        {
            return "登陆成功";
        }
    }
}
