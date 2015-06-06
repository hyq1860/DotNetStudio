using DotNet.CloudFarm.Domain.Model.User;
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
    }
}
