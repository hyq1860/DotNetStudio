using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.CloudFarm.Domain.Model.User;

namespace DotNet.CloudFarm.Domain.ViewModel
{
    public class MyCenterViewModel
    {
        public UserModel User { get; set; }

        /// <summary>
        /// 是否有未读短消息
        /// </summary>
        public bool IsHasNoReadMessage { get; set; }
    }
}
