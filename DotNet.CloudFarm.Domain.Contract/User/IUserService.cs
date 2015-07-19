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
        /// <param name="mobile"></param>
        /// <param name="captcha"></param>
        /// <returns></returns>
        bool CheckMobileCaptcha(string mobile, string captcha);

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
        /// <param name="WxOpenId"></param>
        void UpdateMobileUserByWxOpenId(string mobile, string WxOpenId);
    }
}
