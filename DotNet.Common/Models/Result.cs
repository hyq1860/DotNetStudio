using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Models
{
    /// <summary>
    /// 通用结果
    /// </summary>
    public class Result<T> where T:class
    {
        /// <summary>
        /// 结果状态
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// 需要服务端返回的结果
        /// </summary>
        public T Data { get; set; }
    }
}
