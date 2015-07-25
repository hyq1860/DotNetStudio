using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Models
{
    /// <summary>
    /// 结果状态
    /// </summary>
    [DataContract]
    public class Status
    {
        /// <summary>
        /// 结果状态码 根据业务意义定义
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
