using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.Message
{
    public class MessageModel
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 短消息状态 0 未读 1 已读
        /// </summary>
        public int Status { get; set; }
    }
}
