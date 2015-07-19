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

        /// <summary>
        /// 根据用户名查找用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserModel GetUser(string userName);

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Common.Collections.PagedList<UserModel> GetUserList(int pageIndex, int pageSize);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int UpdateUserStatus(int userId, int status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="captcha"></param>
        /// <param name="sendTime"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool InsertUserCaptcha(int userId,string captcha,DateTime sendTime,int status);

        /// <summary>
        /// 获取用户是否存在没有使用的验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <param name="expireMinute"></param>
        /// <returns></returns>
        string GetUnUsedCaptcha(int userId, string mobile, int expireMinute);

        /// <summary>
        ///  根据OPENID获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        UserModel GetUserByWxOpenId(string openId);

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="wxOpenId"></param>
        void updateMobileByWxOpenId(string mobile, string wxOpenId);
    }
}
