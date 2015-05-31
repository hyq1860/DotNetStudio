using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.WebSite.MVC
{
    /// <summary>
    /// 网站用户
    /// </summary>
    public class WebSiteUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSiteUser"/> class.
        /// </summary>
        /// <param name="userId">
        /// The customer id.
        /// </param>
        public WebSiteUser(int userId)
        {
            this.UserId = userId;
        }

        /// <summary>
        /// Gets CustomerId.
        /// </summary>
        public int UserId { get; private set; }
    }
}
