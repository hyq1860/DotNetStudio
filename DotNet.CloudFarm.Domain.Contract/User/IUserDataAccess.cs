using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.User;

namespace DotNet.CloudFarm.Domain.Contract.User
{
    public interface IUserDataAccess
    {
        void Login();

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns>返回用户userid</returns>
        int Insert(UserModel userModel);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        bool Update(UserModel userModel);

        /// <summary>
        /// 根据userid获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserModel GetUserByUserId(int userId);
    }
}
