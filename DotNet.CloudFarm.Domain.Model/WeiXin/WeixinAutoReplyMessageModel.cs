using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.WeiXin
{
    /// <summary>
    /// 微信关键字自动回复
    /// </summary>
    public class WeixinAutoReplyMessageModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 需要回复的关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 自动回复内容
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 状态：0-删除；1-可用；2-不可用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public int CreatorId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

}
