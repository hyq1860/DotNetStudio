using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Contract;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Impl.User
{
    public class UserService:IUserService
    {
        private IUserDataAccess UserDataAccess;
        public UserService(IUserDataAccess userDataAccess)
        {
            UserDataAccess = userDataAccess;
        }

        public Result<LoginUser> Login(LoginUser loginUser)
        {
            UserDataAccess.Login();
            return new Result<LoginUser>();
        }

        public Result<LoginUser> GetCaptcha(string mobile)
        {
            throw new NotImplementedException();
        }

        public PagedList<MessageModel> GetMessages(int userId, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
