using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DotNet.WebSite.MVC
{
    public class AuthenticationService : IAuthenticationService
    {
        public WebSiteUser GetCurrentUser(HttpContextBase httpContext)
        {
            if (this.Islogged(httpContext))
            {
                var custId = this.GetCustomerId(httpContext);
                return new WebSiteUser(custId);
            }

            return null;
        }

        public bool Islogged(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="httpContext">
        /// The http Context.
        /// </param>
        /// <returns>
        /// CustomerId
        /// </returns>
        private int GetCustomerId(HttpContextBase httpContext)
        {
            // customerID密文
            string customerIdString = httpContext.User.Identity.Name;
            if (String.IsNullOrEmpty(customerIdString))
            {
                return 0;
            }

            int customerId;
            if (!Int32.TryParse(customerIdString, out customerId))
            {
                return 0;
            }

            if (customerId <= 0)
            {
                return 0;
            }

            return customerId;
        }
    }
}
