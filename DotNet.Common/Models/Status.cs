using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Common.Models
{
    /// <summary>
    /// 结果状态
    /// </summary>
    public class Status
    {
        /// <summary>
        /// 结果状态码 根据业务意义定义
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}
