using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.User
{
    public class UserModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 微信用户标识
        /// </summary>
        public string WxOpenId { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WxNickName { get; set; }

        /// <summary>
        /// 微信性别：1时是男性，值为2时是女性，值为0时是未知 
        /// </summary>
        public int WxSex { get; set; }

        /// <summary>
        /// 微信头像URL
        /// </summary>
        public string WxHeadUrl { get; set; }

        /// <summary>
        /// 微信关注时间戳
        /// </summary>
        public DateTime WxSubTime { get; set; }

        /// <summary>
        /// 微信联合id
        /// </summary>
        public string WxUnionId { get; set; }

        /// <summary>
        /// 微信备注
        /// </summary>
        public string WxRemark { get; set; }

        /// <summary>
        /// 微信群组ID
        /// </summary>
        public int WxGroupId { get; set; }

        /// <summary>
        /// 状态：0-禁用；1-可用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
