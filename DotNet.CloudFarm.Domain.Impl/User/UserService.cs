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
        private IUserDataAccess userDataAccess;
        public UserService(IUserDataAccess userDataAccess)
        {
            this.userDataAccess = userDataAccess;
        }

        public Result<LoginUser> Login(LoginUser loginUser)
        {
            userDataAccess.Login();
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

        public UserModel GetUserByUserId(int userId)
        {
            return userDataAccess.GetUserByUserId(userId);
        }

        public int Insert(UserModel userModel)
        {
            return userDataAccess.Insert(userModel);
        }
    }
}
