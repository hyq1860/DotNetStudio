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
        /// <param name="mobile"></param>
        /// <returns></returns>
        Result<LoginUser> GetCaptcha(string mobile);

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

        int Insert(UserModel userModel);
    }
}
