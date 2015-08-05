using DotNet.CloudFarm.Domain.Model.Message;
using DotNet.CloudFarm.Domain.Model.User;
using DotNet.Common.Collections;
using DotNet.Common.Models;

namespace DotNet.CloudFarm.Domain.Contract.User
{
    public interface IUserService
    {
        /// <summary>
        /// 登陆校验接口
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        Result<LoginUser> Login(LoginUser loginUser);

        /// <summary>
        /// 获取验证码接口
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        bool GetCaptcha(int userId, string mobile);

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        bool CheckMobileCaptcha(int userId,string mobile, string captcha);

        /// <summary>
        /// 根据用户id获取用户消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页多少数据</param>
        /// <returns></returns>
        PagedList<MessageModel> GetMessages(int userId,int pageIndex,int pageSize);

        /// <summary>
        /// 根据userid获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserModel GetUserByUserId(int userId);

        /// <summary>
        /// 根据用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserModel GetUser(string userName);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        int Insert(UserModel userModel);

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<UserModel> GetUserList(int pageIndex, int pageSize);

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        int UpdateUserStatus(int userId, int status);


        /// <summary>
        /// 根据微信OPENID获取用户信息
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <returns></returns>
        UserModel GetUserByWxOpenId(string wxOpenId);

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="wxOpenId"></param>
        void UpdateMobileUserByWxOpenId(string mobile, string wxOpenId);

        /// <summary>
        /// 更新验证码的状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        bool UpdateUserCaptchaStatus(int userId,string mobile);

        /// <summary>
        /// 根据用户名和密码找后台管理员model
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        BackstageLoginUser FindByUserNameAndPassword(string userName, string password);

        /// <summary>
        /// 获取后台userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        BackstageLoginUser FindBackstageLoginUserByUserId(int userId);
    }
}
