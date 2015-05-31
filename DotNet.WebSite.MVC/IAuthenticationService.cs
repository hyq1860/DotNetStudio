using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DotNet.WebSite.MVC
{
    /// <summary>
    /// 身份验证接口定义
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        WebSiteUser GetCurrentUser(HttpContextBase httpContext);

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool Islogged(HttpContextBase httpContext);
    }
}
