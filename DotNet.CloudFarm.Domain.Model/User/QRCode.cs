using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.CloudFarm.Domain.Model.User
{
    public class QRCode
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 邀请标识码
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string SourceName { get; set; }


        /// <summary>
        /// 二维码地址
        /// </summary>
        public string QRCodeUrl { get; set; }

        /// <summary>
        /// 状态：0-删除；1-可用；2-不可用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数量统计
        /// </summary>
        public int SourceCount { get; set; }
    }
}
