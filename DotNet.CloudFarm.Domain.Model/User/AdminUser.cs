using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.User
{
    /// <summary>
    /// 后台管理员使用的User
    /// </summary>
    public class AdminUser
    {
        public int Id { get; set; }

        public string AdminName { get; set; }

    }
}
