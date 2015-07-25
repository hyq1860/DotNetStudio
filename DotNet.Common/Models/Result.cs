using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Models
{
    /// <summary>
    /// 通用结果
    /// </summary>
    [DataContract]
    public class Result<T>// where T:class
    {

        /// <summary>
        /// 结果状态
        /// </summary>
        [DataMember]
        public Status Status { get; set; }

        /// <summary>
        /// 需要服务端返回的结果
        /// </summary>
        [DataMember]
        public T Data { get; set; }
    }
}
